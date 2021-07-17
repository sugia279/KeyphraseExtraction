using KeyphraseExtraction.BaseClass;
using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Xml;

namespace KeyphraseExtraction.Model
{
    public class Document : ObservableNotifyObject
    {
        #region Dublin Core Fields
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _filePath = string.Empty;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private string _format = string.Empty;

        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }
        private string _type = string.Empty;

        public string DocType
        {
            get { return _type; }
            set { _type = value; }
        }
        private string _title = string.Empty;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private string _creator = string.Empty;

        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        private string _publisher = string.Empty;

        public string Publisher
        {
            get { return _publisher; }
            set { _publisher = value; }
        }
        private string _contributor = string.Empty;

        public string Contributor
        {
            get { return _contributor; }
            set { _contributor = value; }
        }
        private string _publishedDate = string.Empty;

        public string PublishedDate
        {
            get { return _publishedDate; }
            set { _publishedDate = value; }
        }
        private string _identifier = string.Empty;

        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }
        private string _source = string.Empty;

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        private string _language = string.Empty;

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }
        private string _relation = string.Empty;

        public string Relation
        {
            get { return _relation; }
            set { _relation = value; }
        }
        private string _coverage = string.Empty;

        public string Coverage
        {
            get { return _coverage; }
            set { _coverage = value; }
        }
        private string _right = string.Empty;

        public string Right
        {
            get { return _right; }
            set { _right = value; }
        }
        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion
        #region Fields
        private string _content = string.Empty;
        private Dictionary<int, string> _rawText = new Dictionary<int, string>();
        private Dictionary<int, Sentence> _sentences = new Dictionary<int, Sentence>();
        private DocumentStructure _docStructure = new DocumentStructure();
        private List<string> _assignedKeyphrase = new List<string>();

        public List<string> AssignedKeyphrase
        {
            get { return _assignedKeyphrase; }
            set { _assignedKeyphrase = value; }
        }

        private List<CandidateTerm> _candidateControlledTerms = new List<CandidateTerm>();
        private List<CandidateTerm> _candidateUnControlledTerms = new List<CandidateTerm>();
        private List<CandidateTerm> _assignedCandidateTerms = new List<CandidateTerm>();

        // the numbers for counting the tf, first occurence position
        private int _maxOfTermOccurNumber = 1;
        private int _documentLength = 0;
        #endregion
        #region Properties
        
        public string FileName
        {
            get { return Path.GetFileName(_filePath); }            
        }

        public Dictionary<int, string> RawText
        {
            get { return _rawText; }
            set
            {
                _rawText = value;
            }
        }
        
        public Dictionary<int, Sentence> Sentences
        {
            get { return _sentences; }
            set { _sentences = value; }
        }

        public DocumentStructure DocumentStructure
        {
            get { return _docStructure; }
            set
            {
                _docStructure = value;                
            }
        }

        public List<CandidateTerm> CandidateControlledTerms
        {
            get { return _candidateControlledTerms; }
            set { _candidateControlledTerms = value;
            }
        }
        public List<CandidateTerm> CandidateUncontrolledTerms
        {
            get { return _candidateUnControlledTerms; }
            set
            {
                _candidateUnControlledTerms = value;
            }
        }

        public List<CandidateTerm> AssignedCandidateTerms
        {
            get { return _assignedCandidateTerms; }
            set
            {
                _assignedCandidateTerms = value;
                RaisePropertyChanged(() => AssignedCandidateTerms);
            }
        }
        #endregion
        #region Constructor
        public Document(string filePath, string format, string type)
        {
            _filePath = filePath;
            _content = PDFParser.ExtractTextFromPdf(filePath);
            
            using (System.IO.StringReader reader = new System.IO.StringReader(_content))
            {
                string line;
                int lineNumber = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    RawText.Add(lineNumber,line);
                    lineNumber++;
                }
            }
            Format = format;
            DocType = type;
            DocumentStructure.LoadStructure();
            LoadAssignedKeyphrase();                        
            DocumentExtractionXML.Instance().LoadXML(filePath,this);
            ApplyStructureHandler();
        }

        #endregion

        #region Method

        private void LoadAssignedKeyphrase()
        {
            string keyPathFile = Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath) + ".key");
            List<string> temp = new List<string>();
            if (File.Exists(keyPathFile))
            {
                temp = File.ReadAllLines(keyPathFile).ToList();
                foreach (string k in temp)
                {
                    _assignedKeyphrase.Add(new PorterStemmer().stemTerm(k).ToLower());
                }
            }
        }
        internal bool IsValidStructure()
        {
            return DocumentStructure.IsValidStructure(RawText.Count);
        }

        internal void ApplyStructureHandler()
        {
            NLTKLibPythonProcess.Instance().StartProcess(@".\Scripts\kes.py");
            if (DocumentStructure.ErrorMessage==string.Empty)
            {
                Sentences.Clear();
                _documentLength = 0;
                string unspecifiedParagraph = string.Empty;
                string specifiedParagraph = string.Empty;
                DocumentItem unspecifiedDocItem = DocumentStructure.DocumentItems.FirstOrDefault(x => x.ItemName == "Unspecified");
                for (int line = 1; line <= RawText.Count; line++)
                {
                    DocumentItem definedDocItem = DocumentStructure.DocumentItems.FirstOrDefault(x => x.BeginRow == line);
                    if (definedDocItem != null)
                    {
                        // segment sentence for unspecified paragraph
                        SegmentSentences(unspecifiedParagraph, new DocumentItem(unspecifiedDocItem));
                        unspecifiedParagraph = string.Empty;

                        //get the paragraph is specified
                        for (int line1 = definedDocItem.BeginRow; line1 <= definedDocItem.EndRow; line1++)
                        {
                            if (line1 == definedDocItem.BeginRow)
                            {
                                int itemNameIndex = RawText[line1].ToLower().IndexOf(definedDocItem.ItemName.ToLower());
                                bool isItemName = RawText[line1].Substring(itemNameIndex + definedDocItem.ItemName.Length).Trim().Length == 0;
                                if (isItemName)
                                {
                                    SegmentSentences(RawText[line1], definedDocItem);
                                    continue;
                                }
                            }
                            RawText[line1] = RefineRawTextLine(RawText[line1]);
                            specifiedParagraph += RawText[line1];                            
                        }
                        line = definedDocItem.EndRow;
                        SegmentSentences(specifiedParagraph, definedDocItem);
                        specifiedParagraph = string.Empty;

                    }
                    else
                    {
                        RawText[line] = RefineRawTextLine(RawText[line]);
                        unspecifiedParagraph += RawText[line];
                    }
                }
                if (unspecifiedParagraph != string.Empty)
                {
                    SegmentSentences(unspecifiedParagraph, new DocumentItem(unspecifiedDocItem));
                    unspecifiedParagraph = string.Empty;
                }
            }
            SpecifyDocumentTitle();
        }

        private string RefineRawTextLine(string line)
        {
            if (line.LastIndexOf(" ") != line.Length - 1)
            {
                line += " ";
            }

            if (line.TrimEnd().LastIndexOf("-") > -1 && line.TrimEnd().LastIndexOf("-") == (line.TrimEnd().Length - 1))
            {
                line = line.TrimEnd().Remove(line.TrimEnd().Length - 1);
            }
            return line;
        }


        internal void SegmentSentences(string paragraph, DocumentItem section)
        {
            List<string> sentences = NLTKLibPythonProcess.Instance().SegmentSentences(paragraph);

            int i = Sentences.Count + 1;
            foreach (string sentence in sentences)
            {
                // calculate number of all word in document
                _documentLength += StringProcessor.CountWords1(sentence);
                Sentences.Add(i, new Sentence(sentence, section));
                i++;
            }
        }
        
        private void SpecifyDocumentTitle()
        {
            KeyValuePair<int, Sentence> title = Sentences.FirstOrDefault(x=>x.Value.DocItemOwner.ItemName == "Title");
            if(title.Value!=null)
                Title = title.Value.Content;
        }

        public void ExtractCandidate()
        {
            List<string> sentCanTerms = new List<string>();
            int termCount = 0;           
            CandidateControlledTerms.Clear();
            CandidateUncontrolledTerms.Clear();
            AssignedCandidateTerms.Clear();
            foreach (var pairSent in Sentences)
            {
                // get each sentence for extracting
                sentCanTerms = pairSent.Value.ExtractCandidateTerm();
                foreach (string term in sentCanTerms)
                {
                    termCount++;
                    string stemmedTerm = new PorterStemmer().stemTerm(term).ToLower();
                    // check whether term is in controlled term collection or not
                    CandidateTerm controlledTerm = _candidateControlledTerms.FirstOrDefault(x => x.StemmedTerm == stemmedTerm);
                    if (controlledTerm != null)
                    {
                        // update features for extracted controlled term
                        UpdateCandidateProperties(controlledTerm, term, pairSent.Value.DocItemOwner);
                        pairSent.Value.DocItemOwner.CandidateTerms.Add(controlledTerm);
                    }
                    else
                    {
                        CandidateTerm uncontrolledTerm = _candidateUnControlledTerms.FirstOrDefault(x => x.StemmedTerm == stemmedTerm);
                        if (uncontrolledTerm != null)
                        {
                            // update features for extracted uncontrolled term
                            UpdateCandidateProperties(uncontrolledTerm, term, pairSent.Value.DocItemOwner);
                            pairSent.Value.DocItemOwner.CandidateTerms.Add(uncontrolledTerm);
                        }
                        else
                        {
                            // create new term
                            CandidateTerm newTerm = CreateNewCandidateTerm(term, stemmedTerm, termCount, pairSent.Value.DocItemOwner);

                            if (newTerm.ControlledTerm)
                            {
                                CandidateControlledTerms.Add(newTerm);
                            }
                            else
                            {
                                CandidateUncontrolledTerms.Add(newTerm);
                            }
                            pairSent.Value.DocItemOwner.CandidateTerms.Add(newTerm);
                        }                        
                    }
                }
            }            
            
            // Update tf feature for candidate term
            foreach (CandidateTerm term in CandidateControlledTerms)
            {
                term.Tf = (double)term.OccurenceNumber / (double)_maxOfTermOccurNumber;
            }

            foreach (CandidateTerm term in CandidateUncontrolledTerms)
            {
                term.Tf = (double)term.OccurenceNumber / (double)_maxOfTermOccurNumber;
            }

            CandidateControlledTerms = new List<CandidateTerm>(CandidateControlledTerms.OrderBy(x => x.StemmedTerm));
            CandidateUncontrolledTerms = new List<CandidateTerm>(CandidateUncontrolledTerms.OrderBy(x => x.StemmedTerm));
        }

        private CandidateTerm CreateNewCandidateTerm(string term, string stemmedTerm, int termCount, DocumentItem docItem)
        {
            Dictionary<string, object> fs = PrepareTermFeatures(stemmedTerm);
            string id = (string)fs["id_term"];
            bool isControlledTerm = (bool)fs["ControlledTerm"];
            double tf = 0;
            double idf = (double)fs["idf"];
            double firstOccurencePosition = (double)termCount / (double)_documentLength;
            int lengthTerm = StringProcessor.CountWords1(term);
            double nodeDegree = (double)fs["NodeDegree"];
            double occurencePositionWeight = docItem.Weight;
            // create new candidateTerm object
            CandidateTerm newTerm = new CandidateTerm(id, term, stemmedTerm, isControlledTerm, tf,idf, firstOccurencePosition, lengthTerm, nodeDegree, occurencePositionWeight);
            newTerm.IdKeyphrase = (string)fs["IdKeyphrase"];            
            newTerm.OccurenceNumber = 1;
            newTerm.DocItemOwners.Add(docItem);
            // register this event before changing the value of IsKeyphrase property
            newTerm.PropertyChanged += newTerm_PropertyChanged;
            //update "checked" to the assigned keyphrases by manual from file
            if (_assignedKeyphrase.FirstOrDefault(x => x.ToLower() == stemmedTerm) != null)
            {
                newTerm.IsKeyphrase = true;
            }
            return newTerm;
        }

        private Dictionary<string, object> PrepareTermFeatures(string stemmedTerm)
        {
            Dictionary<string, object> features = new Dictionary<string, object>();
            
            // check existence of id term in DB
            Dictionary<string, string> existTerm = DocumentExtractionDB.Instance().GetTermByStemmedTerm(stemmedTerm);
            features.Add("id_term", existTerm != null ? existTerm["id_term"]: null);
            
            //stem term and check with ontology
            Dictionary<string, string> controlled = OntologyDB.Instance().GetKeyphrasesByStemmedKeyphrase(stemmedTerm);
            features.Add("ControlledTerm", controlled != null);
            features.Add("IdKeyphrase", controlled != null ? controlled["id_keyphrase"] : string.Empty);
                
            int docAllTotal = DocumentExtractionDB.Instance().Documents.Count + 1; // +1 for current doc
            int docContainTermTotal = DocumentExtractionDB.Instance().GetDocumentNumberContainTerm((string)features["id_term"]);
            features.Add("idf",Math.Log((double)(docAllTotal)/(double)(1 + docContainTermTotal)));

            double nodeDegree = 0;            
            if (controlled != null)
            {
                nodeDegree = OntologyDB.Instance().GetNodeDegreeStemmedKeyphrase(controlled["id_keyphrase"]);
            }
            features.Add("NodeDegree", nodeDegree);
            
            return features;
        }

        private void newTerm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CandidateTerm changedTerm = (CandidateTerm)sender;
            if (AssignedCandidateTerms.Contains(changedTerm) && !changedTerm.IsKeyphrase)
            {
                AssignedCandidateTerms.Remove(changedTerm);
            }
            else if (!AssignedCandidateTerms.Contains(changedTerm) && changedTerm.IsKeyphrase)
            {
                AssignedCandidateTerms.Add(changedTerm);
            }
            AssignedCandidateTerms = new List<CandidateTerm>(AssignedCandidateTerms.OrderByDescending(o => o.ControlledTerm).ToList());            
        }

        private void UpdateCandidateProperties(CandidateTerm oldCanTerm, string newTerm, DocumentItem docItem)
        {
            if (oldCanTerm.Terms.Contains(newTerm) == false)
            {
                oldCanTerm.Terms = oldCanTerm.Terms+ "\n" + newTerm;
            }
            // calculate Occurence number and occurrence position weight
            oldCanTerm.OccurenceNumber++;
            if (oldCanTerm.DocItemOwners.Contains(docItem) == false)
            {
                oldCanTerm.DocItemOwners.Add(docItem);
                oldCanTerm.OccurencePositionWeight = oldCanTerm.OccurencePositionWeight + docItem.Weight;
            }
            if (oldCanTerm.OccurenceNumber > _maxOfTermOccurNumber)
            {
                _maxOfTermOccurNumber = oldCanTerm.OccurenceNumber;
            }

            //update "checked" to the assigned keyphrases by manual from file
            if (_assignedKeyphrase.FirstOrDefault(x => x.ToLower() == oldCanTerm.StemmedTerm) != null && oldCanTerm.IsKeyphrase == false)
            {
                oldCanTerm.IsKeyphrase = true;
            }
        }

        
        internal bool GenerateToXMLFile()
        {
            bool saveSuccess = true;
            DocumentExtractionXML.Instance().SetDocument(this);
            DocumentExtractionXML.Instance().SaveToXML(Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FileName) + ".xml"));

            return saveSuccess;
        }

        public bool SaveToDB()
        {
            bool saveSuccess = true;
            saveSuccess = DocumentExtractionDB.Instance().InsertDocument(this);
            if (saveSuccess)
                MessageBox.Show("The save processing is sucessful.");
            return saveSuccess;
        }

        public void ClearAssignedKeyphrases()
        {
            List<CandidateTerm> temp = new List<CandidateTerm>(AssignedCandidateTerms);
            foreach (CandidateTerm t in temp)
            {
                t.IsKeyphrase = false;
            }
            AssignedCandidateTerms.Clear();
        }

        public void RemoveAssignedKeyphrases(CandidateTerm removeTerm)
        {
            removeTerm.IsKeyphrase = false;
        }
        #endregion


    }
}
