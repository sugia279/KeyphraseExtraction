using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class DocumentExtractionDB
    {
        private static DocumentExtractionDB _instance;

        public static DocumentExtractionDB Instance()
        {
            if (_instance == null)
            {
                _instance = new DocumentExtractionDB();
            }
            return _instance;
        }

        private ArrayList _documents;

        public ArrayList Documents
        {
            get { return _documents; }
            set { _documents = value; }
        }

        private ArrayList _candidateTerms;

        public ArrayList CandidateTerms
        {
            get { return _candidateTerms; }
            set { _candidateTerms = value; }
        }

        private ArrayList _documentTermRel;

        public ArrayList DocumentTermRel
        {
            get { return _documentTermRel; }
            set { _documentTermRel = value; }
        }

        private ArrayList _documentSections;

        public ArrayList DocumentSections
        {
            get { return _documentSections; }
            set { _documentSections = value; }
        }
        private ArrayList _documentStructure;

        public ArrayList DocumentStructure
        {
            get { return _documentStructure; }
            set { _documentStructure = value; }
        }

        private ArrayList _trainingData;

        public ArrayList TrainingData
        {
            get { return _trainingData; }
            set { _trainingData = value; }
        }

        private ArrayList _keyphraseAssignmentProbability;

        public ArrayList KeyphraseAssignmentProbability
        {
            get { return _keyphraseAssignmentProbability; }
            set { _keyphraseAssignmentProbability = value; }
        }

        public DocumentExtractionDB()
        { }

        public void LoadDocumentExtractionDB()
        {
            DBUtilities.OpenConnection("Document_Extraction");
            Documents = DBUtilities.ExecuteReaderQuery("select * from Document");
            CandidateTerms = DBUtilities.ExecuteReaderQuery("select * from candidate_term");
            DocumentTermRel = DBUtilities.ExecuteReaderQuery("select * from document_terms_relationship");
            DocumentSections = DBUtilities.ExecuteReaderQuery("select * from document_sections");
            DocumentStructure = DBUtilities.ExecuteReaderQuery("select * from document_structure");
            KeyphraseAssignmentProbability = DBUtilities.ExecuteReaderQuery("select * from keyphrase_assignment_probability");
            DBUtilities.CloseConnection();
        }

        private DataTable GetTrainningData(ref int keyphraseCount)
        {
            DBUtilities.OpenConnection("Document_Extraction");
            DataTable table = new DataTable();
            table.Columns.Add("Label");
            table.Columns.Add("Tfidf", typeof(double));
            table.Columns.Add("OccurFirstPos", typeof(double));
            table.Columns.Add("OccurPosWeight", typeof(double));
            table.Columns.Add("LengthTerm", typeof(double));
            table.Columns.Add("NodeDegree", typeof(double));
            TrainingData = DBUtilities.ExecuteReaderQuery("SELECT is_keyphrase, (tf*idf) as tfidf, occurence_first_position, occurence_position_weight, length_term, node_degree" + 
                                                            " FROM document_extraction.document_terms_relationship,document_extraction.candidate_term"+
                                                            " Where document_extraction.document_terms_relationship.id_term=document_extraction.candidate_term.id_term" +
                                                            " Order by is_keyphrase");
            string Label = string.Empty;
            double Tfidf = 0.0;
            double OccurFirstPos = 0.0;
            double OccurPosWeight = 0.0;
            double LengthTerm = 0.0;
            double NodeDegree = 0.0;


            foreach (Dictionary<string, string> kDict in TrainingData)
            {
                Label = kDict["is_keyphrase"] == "0"? "IsNotKeyphrase": "IsKeyphrase";
                keyphraseCount = kDict["is_keyphrase"] == "1" ? keyphraseCount + 1 : keyphraseCount;
                Tfidf= double.Parse(kDict["tfidf"]);
                OccurFirstPos = double.Parse(kDict["occurence_first_position"]);
                OccurPosWeight = double.Parse(kDict["occurence_position_weight"]);
                LengthTerm = double.Parse(kDict["length_term"]);
                NodeDegree = double.Parse(kDict["node_degree"]);

                table.Rows.Add(Label, Tfidf, OccurFirstPos, OccurPosWeight, LengthTerm, NodeDegree);

            }
            DBUtilities.CloseConnection();
            return table;
        }

        public void TrainningData()
        {
            int keyphraseCount = 0;
            DataTable trainingData = GetTrainningData(ref keyphraseCount);
            DataTable keyphraseAssignmentProbability = NaiveBayesClassifier.Instance().TrainClassifier(trainingData);
            DBUtilities.OpenConnection("Document_Extraction");
            InsertGaussianClassifierData(keyphraseAssignmentProbability, keyphraseCount, trainingData.Rows.Count - keyphraseCount);
            KeyphraseAssignmentProbability = DBUtilities.ExecuteReaderQuery("select * from keyphrase_assignment_probability");
            DBUtilities.CloseConnection();
        }

        private int InsertGaussianClassifierData(DataTable table, int keyphraseCount, int notKeyphraseCount)
        {   
            // truncate old data
            DBUtilities.ExecuteNonReader("Truncate keyphrase_assignment_probability;");
            // insert query
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            string query = string.Empty;
            int returnVal  = 0;
            foreach (DataRow row in table.Rows)
            {
                columnData = new Dictionary<string, string>();
                columnData.Add("Label", (string)row["label"]);
                columnData.Add("terms_amount", (row["label"] == "IsKeyphrase" ? keyphraseCount : notKeyphraseCount).ToString());
                
                columnData.Add("mean_tfidf", (string)row["TfidfMean"]);
                columnData.Add("variance_tfidf", (string)row["TfidfVariance"]);
                columnData.Add("mean_occurence_first_posstion", (string)row["OccurFirstPosMean"]);
                columnData.Add("variance_occurence_first_posstion", (string)row["OccurFirstPosVariance"]);
                columnData.Add("mean_occurence_posstion_weight", (string)row["OccurPosWeightMean"]);
                columnData.Add("variance_occurence_posstion_weight", (string)row["OccurPosWeightVariance"]);
                columnData.Add("mean_length_term", (string)row["LengthTermMean"]);
                columnData.Add("variance_length_term", (string)row["LengthTermVariance"]);
                columnData.Add("mean_node_degree", (string)row["NodeDegreeMean"]);
                columnData.Add("variance_node_degree", (string)row["NodeDegreeVariance"]);
                query = DBUtilities.InsertQueryString("keyphrase_assignment_probability", columnData);
                returnVal = DBUtilities.ExecuteNonReader(query);
            }
            //string query = ;
            
            return returnVal;
        }

        public DataTable GetProbabilityDataTable(ref int positveTrainedTermsCount, ref int negativeTrainedTermsCount)
        {
            DataTable table = new DataTable();
            foreach (Dictionary<string, string> row in KeyphraseAssignmentProbability)
            {
                foreach (var pair in row)
                {
                    if (!table.Columns.Contains(pair.Key) && pair.Key != "idTrainingData")
                    {
                        if (pair.Key.ToLower() == "terms_amount")
                        {
                            if (row["label"] == "IsKeyphrase")
                            {
                                positveTrainedTermsCount = int.Parse(row["terms_amount"]);
                            }
                            else
                                negativeTrainedTermsCount = int.Parse(row["terms_amount"]);
                            continue;
                        }
                        if (pair.Key.ToLower() == "label")
                        {
                            table.Columns.Add(pair.Key);
                        }
                        else
                            table.Columns.Add(pair.Key, typeof(double));
                    }
                }
                table.Rows.Add(row["label"], row["mean_tfidf"], row["variance_tfidf"],
                                row["mean_occurence_first_posstion"], row["variance_occurence_first_posstion"],
                                row["mean_occurence_posstion_weight"], row["variance_occurence_posstion_weight"],
                                row["mean_length_term"], row["variance_length_term"],
                                row["mean_node_degree"], row["variance_node_degree"]);
            }
            return table;
        }

        public Dictionary<string, string> GetTermByStemmedTerm(string stemmedTerm)
        {
            Dictionary<string, string> kp = null;
            foreach (Dictionary<string, string> kDict in CandidateTerms)
            {
                if (kDict["stemmed_term"].ToLower() == stemmedTerm)
                {
                    kp = kDict;
                    break;
                }

            }
            return kp;
        }

        public int GetDocumentNumberContainTerm(string id)
        {
            int docNumber = 0;
            if (id != null)
            {
                foreach (Dictionary<string, string> docT in DocumentTermRel)
                {
                    if (docT["id_term"] == id)
                    {
                        docNumber++;
                    }
                }
            }
            return docNumber;
        }

        internal bool InsertDocument(Document document)
        {
            DBUtilities.OpenConnection("Document_Extraction");

            // insert document
            string query = CreateDocumentInsertQuery(document);
            
            int returnVal = DBUtilities.ExecuteNonReader(query);
            ArrayList queryData = DBUtilities.ExecuteReaderQuery("select @@IDENTITY as id_Document from document");
            document.Id = (queryData[0] as Dictionary<string, string>)["id_Document"];

            // insert cadidate terms
            List<Tuple<CandidateTerm, string>> query1 = CreateTermInsertUpdateString(document);

            foreach (Tuple<CandidateTerm, string> q in query1)
            {
                returnVal = DBUtilities.ExecuteNonReader(q.Item2);
                if (returnVal > -1)
                {
                    if (q.Item1.Id == null)
                    {
                        queryData = DBUtilities.ExecuteReaderQuery("select @@IDENTITY as id_candidate_term from candidate_term");
                        q.Item1.Id = (queryData[0] as Dictionary<string, string>)["id_candidate_term"];
                    }

                    query = CreateDocTermRelInsertString(document, q.Item1);
                    returnVal = DBUtilities.ExecuteNonReader(query);
                }
            }

            // insert relationship

            if (returnVal > -1)
            {
                DBUtilities.CloseConnection();
            }
            return returnVal > -1;
        }

        private string CreateDocumentInsertQuery(Document document)
        {
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            columnData.Add("title", RefineInvalidData(document.Title));
            columnData.Add("id_type", "1");
            columnData.Add("id_subject", "141");
            columnData.Add("file_name", RefineInvalidData(document.FileName));
            columnData.Add("creator", RefineInvalidData(document.Creator));
            columnData.Add("description", RefineInvalidData(document.Description));
            columnData.Add("publisher", RefineInvalidData(document.Publisher));
            columnData.Add("contributor", RefineInvalidData(document.Contributor));
            columnData.Add("published_date", RefineInvalidData(document.PublishedDate));
            columnData.Add("format", RefineInvalidData(document.Format));
            columnData.Add("identifier", RefineInvalidData(document.Identifier));
            columnData.Add("language", RefineInvalidData(document.Language));
            columnData.Add("relation", RefineInvalidData(document.Relation));
            columnData.Add("coverage", RefineInvalidData(document.Coverage));
            columnData.Add("rights", RefineInvalidData(document.Right));
            string query = DBUtilities.InsertQueryString("document", columnData);
            return query;
        }

        private List<Tuple<CandidateTerm, string>> CreateTermInsertUpdateString(Document document)
        {
            List<Tuple<CandidateTerm, string>> termQuery = new List<Tuple<CandidateTerm, string>>();
            List<CandidateTerm> savedCandidateTerms = document.CandidateControlledTerms.ToList();
            savedCandidateTerms.AddRange(document.AssignedCandidateTerms.Where(x => x.ControlledTerm == false).ToList());

            Dictionary<string, string> columnData = new Dictionary<string, string>();
            Dictionary<string, string> whereFields = new Dictionary<string, string>();
            string insertQuery = "", updateQuery = "";
            ArrayList queryData = new ArrayList();
            foreach (CandidateTerm t in savedCandidateTerms)
            {
                columnData.Clear();
                whereFields.Clear();
                if (t.Id == null)
                {
                    columnData.Add("stemmed_term", RefineInvalidData(t.StemmedTerm));
                    columnData.Add("term", RefineInvalidData(t.Terms));
                    columnData.Add("is_controlled_term", t.ControlledTerm ? "1" : "0");
                    columnData.Add("idf", t.Idf.ToString());
                    columnData.Add("length_term", t.LenghtWord.ToString());
                    columnData.Add("node_degree", t.NodeDegree.ToString());
                    insertQuery = DBUtilities.InsertQueryString("candidate_term", columnData);
                    Tuple<CandidateTerm, string> term = new Tuple<CandidateTerm, string>(t, insertQuery);
                    termQuery.Add(term);
                }
                else
                {
                    columnData.Add("is_controlled_term", t.ControlledTerm ? "1" : "0");
                    columnData.Add("idf", t.Idf.ToString());
                    columnData.Add("length_term", t.LenghtWord.ToString());
                    columnData.Add("node_degree", t.NodeDegree.ToString());

                    whereFields.Add("id_term", t.Id);
                    updateQuery = DBUtilities.UpdateQueryString("candidate_term", columnData, whereFields);
                    Tuple<CandidateTerm, string> term = new Tuple<CandidateTerm, string>(t, updateQuery);
                    termQuery.Add(term);
                }
            }
            return termQuery;
        }

        private string CreateDocTermRelInsertString(Document doc, CandidateTerm term)
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("id_document", doc.Id);
            fields.Add("id_term", term.Id);
            fields.Add("is_keyphrase", term.IsKeyphrase?"1":"0");
            fields.Add("tf", term.Tf.ToString());
            fields.Add("occurence_first_position", term.FirstOccurencePosition.ToString());
            fields.Add("occurence_position_weight", term.OccurencePositionWeight.ToString());
            return DBUtilities.InsertQueryString("document_terms_relationship", fields);
        }
        
        private string RefineInvalidData(string value)
        {
            if(value!=null)
                return value.Replace("'", "''");
            return null;
        }
    }
}
