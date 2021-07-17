using KeyphraseExtraction.BaseClass;
using KeyphraseExtraction.Model;
using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Data;

namespace KeyphraseExtraction.ViewModel
{
    public class MainViewModel : ObservableNotifyObject
    {
        #region fields
        MainWindow _keyphraseExtractionView;
        TrainingWindow _trainingView;
        RelayCommands _openFolderBrowserDialogCommand;
        RelayCommands _extractCandidateCommand;
        RelayCommands _extractKeyphraseCommand;
        RelayCommands _trainingCommand;
        RelayCommands _updateOntologyCommand;
        RelayCommands _closeCommand;
        RelayCommands _prevDocumentCommand;
        RelayCommands _nextDocumentCommand;
        RelayCommands _searchTextCommand;
        RelayCommands _searchTermCommand;
        RelayCommands _applyStructureCommand;
        RelayCommands _resetCommand;
        RelayCommands _saveCommand;
        RelayCommands _deleteCommand;
        RelayCommands _deleteKeyphraseCommand;

        private string _folderPath = string.Empty;
        private List<string> _fileNameCollection = new List<string>();
        private string _selectedFile = string.Empty;
        private ObservableCollection<Document> _documentCollection = new ObservableCollection<Document>();
        private Document _curDocument;
        private Dictionary<int, string> _contentLines = new Dictionary<int,string>();
        private ObservableCollection<CandidateTerm> _candidateTerms = new ObservableCollection<CandidateTerm>();        

        private string _searchText = string.Empty;
        private string _searchTerm = string.Empty;
        private bool _isRawText = false;
        private bool _isSentences = false;
        private bool _isControlled = false;
        private bool _isUncontrolled = false;
        private IList _selectedTermItems = new ArrayList();
        private IList _selectedLines = new ArrayList();
        private IList _selectedKeyphraseItems = new ArrayList();

        #endregion

        #region constructor
        public MainViewModel(MainWindow view)
        {
            _keyphraseExtractionView = view;            
            view.DataContext = this;
            OntologyDB.Instance().LoadDB();
            DocumentExtractionDB.Instance().LoadDocumentExtractionDB();
        }

        #endregion
        #region properties
        
        public string FolderPath
        {
            get
            {
                return _folderPath;
            }
            set
            {
                _folderPath = value;
                base.RaisePropertyChanged(() => FolderPath);
            }
        }
       
        public List<string> FileNameCollection
        {
            get
            {
                return _fileNameCollection;
            }
            set
            {
                _fileNameCollection = value;                
                RaisePropertyChanged(() => FileNameCollection);
                RaisePropertyChanged(() => TotalDocument);
            }
        }
                
        public string SelectedFile
        {
            get
            {
                return _selectedFile;
            }
            set
            {
                _selectedFile = value;
                // find document exist or not in document collection
                var docVar=_documentCollection.FirstOrDefault(x=>Path.GetFileName(x.FilePath)==_selectedFile);
                if (docVar == null)
                {
                    CurrentDocument = new Document(Path.Combine(_folderPath, _selectedFile), "pdf", "paper");
                    _documentCollection.Add(CurrentDocument);
                    CandidateTerms.Clear();
                }
                else
                {
                    CurrentDocument = (Document)docVar;
                    ControlledChecked = true;
                }
                RawTextChecked = true;
                base.RaisePropertyChanged(() => SelectedFile);
            }
        }

        public Document CurrentDocument
        {
            get { return _curDocument; }
            set
            {
                _curDocument = value;
                RaisePropertyChanged(() => CurrentDocument);                
            }
        }

        public Dictionary<int,string> ContentLines
        {
            get { return _contentLines; }
            set
            {
                _contentLines = value;
                RaisePropertyChanged(() => ContentLines);
            }
        }

        public string TotalDocument
        {
            get { return FileNameCollection.Count.ToString(); }
        }


        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value.ToLower(); }          
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { _searchTerm = value.ToLower(); }
        }

        public bool RawTextChecked
        {
            get { return _isRawText; }
            set
            {
                _isRawText = value;
                if (_isRawText && CurrentDocument!=null)
                {
                    ContentLines.Clear();
                    ContentLines=CurrentDocument.RawText.ToDictionary(x=>x.Key,x=>x.Value);
                }
                RaisePropertyChanged(() => RawTextChecked);
            }    
        }

        public bool SentencesChecked
        {
            get { return _isSentences; }
            set 
            {
                _isSentences = value;
                if (_isSentences && CurrentDocument != null)
                {
                    ContentLines.Clear();
                    ContentLines = CurrentDocument.Sentences.ToDictionary(x=>x.Key,x=>x.Value.Content);
                }
                RaisePropertyChanged(() => SentencesChecked);
            }
        }


        public ObservableCollection<CandidateTerm> CandidateTerms
        {
            get { return _candidateTerms; }
            set
            {
                _candidateTerms = value;
                RaisePropertyChanged(() => CandidateTerms);
            }
        }


        public IList SelectedTermItems
        {
            get { return _selectedTermItems; }
            set
            {
                _selectedTermItems = value;
                RaisePropertyChanged(() => SelectedTermItems);
            }
        }

        public IList SelectedLines
        {
            get { return _selectedLines; }
            set
            {
                _selectedLines = value;
                RaisePropertyChanged(() => SelectedLines);
            }
        }

        public IList SelectedKeyphraseItems
        {
            get { return _selectedKeyphraseItems; }
            set
            {
                _selectedKeyphraseItems = value;
                RaisePropertyChanged(() => SelectedKeyphraseItems);
            }
        }

        public bool ControlledChecked
        {
            get { return _isControlled; }
            set
            {
                _isControlled = value;
                if (_isControlled && CurrentDocument != null)
                {
                    CandidateTerms.Clear();
                    CandidateTerms = new ObservableCollection<CandidateTerm>(CurrentDocument.CandidateControlledTerms);
                }
                RaisePropertyChanged(() => ControlledChecked);
            }
        }

        public bool UncontrolledChecked
        {
            get { return _isUncontrolled; }
            set
            {
                _isUncontrolled = value;
                if (_isUncontrolled && CurrentDocument != null)
                {
                    CandidateTerms.Clear();
                    CandidateTerms = CandidateTerms = new ObservableCollection<CandidateTerm>(CurrentDocument.CandidateUncontrolledTerms);
                }
                RaisePropertyChanged(() => UncontrolledChecked);
            }
        }


        private bool _checkAll = false;
        
        public bool CheckAll
        {
            get
            {
                _checkAll = false;

                foreach (CandidateTerm term in CandidateTerms)
                {
                    if (!term.IsKeyphrase)
                    {
                        _checkAll = false;
                        break;
                    }
                    _checkAll = true;
                }

                return _checkAll;
            }
            set
            {
                foreach (var member in CandidateTerms)
                    member.IsKeyphrase = value;

                _checkAll = value;                
            }
        }
        
        public int TotalAssignedKeyphrase 
        {
            get {
                int number = 0;
                foreach (var member in CandidateTerms)
                {
                    if (member.IsKeyphrase)
                    {
                        number++;
                    }
                }
                return number;
            }            
        }
        #endregion
        #region Commands
        public ICommand OpenDialogCommand
        {
            get
            {
                return _openFolderBrowserDialogCommand ?? (_openFolderBrowserDialogCommand = new RelayCommands(() => OpenDialogHandler()));
            }
        }

        public ICommand ExtractCandidateCommand
        {
            get
            {
                return _extractCandidateCommand ?? (_extractCandidateCommand = new RelayCommands(() => ExtractCandidateHandler()));
            }
        }

        public ICommand ExtractKeyphraseCommand
        {
            get
            {
                return _extractKeyphraseCommand ?? (_extractKeyphraseCommand = new RelayCommands(() => ExtractKeyphraseHandler()));
            }
        }

        public RelayCommands SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommands(() => SaveDocHandler()));
            }
        }

        public ICommand TrainingCommand
        {
            get
            {
                return _trainingCommand ?? (_trainingCommand = new RelayCommands(() => TrainingHandler()));
            }
        }
        
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommands(() => CloseHandler()));
            }
        }

        public ICommand PreviousDocument
        {
            get
            {
                return _prevDocumentCommand ?? (_prevDocumentCommand = new RelayCommands(() => ChangeDocument("previous")));
            }
        }

        public ICommand NextDocument
        {
            get
            {
                return _nextDocumentCommand ?? (_nextDocumentCommand = new RelayCommands(() => ChangeDocument("next")));
            }
        }

        public ICommand SearchTextCommand
        {
            get
            {
                return _searchTextCommand ?? (_searchTextCommand = new RelayCommands(() => SearchTextHandler()));
            }
        }

        public ICommand SearchTermCommand
        {
            get
            {
                return _searchTermCommand ?? (_searchTermCommand = new RelayCommands(() => SearchTermHandler()));
            }
        }


        public ICommand ApplyStructureCommand
        {
            get
            {
                return _applyStructureCommand ?? (_applyStructureCommand = new RelayCommands(() => ApplyStructureCommandHanlder()));
            }            
        }

        public RelayCommands DeleteTermsCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommands(() => DeleteTermsCommandHandler()));
            }   
        }

        public RelayCommands DeleteKeyphrasesCommand
        {
            get
            {
                return _deleteKeyphraseCommand ?? (_deleteKeyphraseCommand = new RelayCommands(() => DeleteKeyphrasesCommandHandler()));
            }
        }

        public RelayCommands ResetCommand
        {
            get
            {
                return _resetCommand ?? (_resetCommand = new RelayCommands(() => ResetCommandHandler()));
            }   
        }

        #endregion

        #region Event handler

        private void OpenDialogHandler()
        {
            OpenFolderBrowserDialog();
            if (Directory.Exists(_folderPath))
            {                
                List<string> filelist = Directory.GetFiles(_folderPath, "*.pdf",
                                         SearchOption.TopDirectoryOnly).ToList<string>();
                if (filelist != null && filelist.Count > 0)
                {
                    //Fill all file names into file combobox
                    FileNameCollection = filelist.Select(x => Path.GetFileName(x)).ToList<string>();
                    SelectedFile = _fileNameCollection[0];
                }
            }
        }

        private void ExtractCandidateHandler()
        {
            if (CurrentDocument != null)
            {
                CurrentDocument.ExtractCandidate();
                ControlledChecked = true;
                UncontrolledChecked = false;
            }
            else
                MessageBox.Show("Please select a document before extract candidate");
        }
        
        private void ExtractKeyphraseHandler()
        {
            if (CurrentDocument.CandidateControlledTerms.Count == 0)
            {
                DialogResult btn = MessageBox.Show("You have not extracted candidate terms of this document yet. Would you like to extract the document's candidate terms?", "Keyphrase Extraction", MessageBoxButtons.OKCancel);
                if (btn == DialogResult.OK)
                {
                    ExtractCandidateHandler();
                }
                else
                    return;
            }
            //DocumentItem keywordsSection = CurrentDocument.DocumentStructure.DocumentItems.First(x => x.ItemName == "Keywords");
            List<Tuple<CandidateTerm,double>> filteredTerms = FilterCanTermByMachineLearning();
            Dictionary<int, Class> keyphraseExtractionDict = FilterCanTermByPotentialClasses(filteredTerms);
            KeyphraseExtractionResultWindow kew = new KeyphraseExtractionResultWindow();
            KeyphraseExtractionResultViewModel kevm = new KeyphraseExtractionResultViewModel(kew, keyphraseExtractionDict, CurrentDocument.AssignedCandidateTerms);            
            kew.ShowDialog();

            if (kevm.SelectedKeyphrases.Count > 0)
            {
                CurrentDocument.ClearAssignedKeyphrases();
                foreach (CandidateTerm t in kevm.SelectedKeyphrases)
                {
                    t.IsKeyphrase = true;
                }
            }
        }


        private void SaveDocHandler()
        {
            DocumentSavingWindow saveview = new DocumentSavingWindow();
            DocumentSavingViewModel docSavVM = new DocumentSavingViewModel(saveview, CurrentDocument);

            saveview.ShowDialog();
            if (docSavVM.SaveToTrainPressed == ButtonTypePressed.Save)
            {
                CurrentDocument.SaveToDB();
                DocumentExtractionDB.Instance().LoadDocumentExtractionDB();
            }
            if (docSavVM.GeneratePressed == ButtonTypePressed.GenerateFile)
            {
                CurrentDocument.GenerateToXMLFile();
            }
        }
        
        private void TrainingHandler()
        {
            //_trainingView = new TrainingWindow();
            //_trainingView.ShowDialog();
            DocumentExtractionDB.Instance().TrainningData();

        }
        
        private void ChangeDocument(string p)
        {
            if (p == "previous")
            {
                int i = FileNameCollection.FindIndex(x=>x==SelectedFile);
                if (i > 0)
                {
                    SelectedFile = FileNameCollection[i - 1];
                }                
            }
            else if (p == "next")
            {
                int i = FileNameCollection.FindIndex(x => x == SelectedFile);
                if (i < FileNameCollection.Count - 1)
                {
                    SelectedFile = FileNameCollection[i + 1];
                }    
            }
        }


        private void SearchTextHandler()
        {
            SelectedLines.Clear();
            foreach(var pair in ContentLines)
            {
                if (pair.Value.ToLower().Contains(SearchText))
                {
                    SelectedLines.Add(pair);
                }
            }
            RaisePropertyChanged(() => SelectedLines);
        }

        private void SearchTermHandler()
        {  
            SelectedTermItems.Clear();
            for (int t = 0; t < CandidateTerms.Count; t++)
            {
                if (CandidateTerms[t].Terms.Contains(SearchTerm))
                {
                    SelectedTermItems.Add(CandidateTerms[t]);
                }
            }
            RaisePropertyChanged(() => SelectedTermItems);
        }
        
        private void ApplyStructureCommandHanlder()
        {
            if (CurrentDocument != null)
            {
                if (CurrentDocument.IsValidStructure())
                {
                    CurrentDocument.ApplyStructureHandler();
                    RawTextChecked = false;
                    SentencesChecked = true;
                }
            }                
        }
        
        private void DeleteTermsCommandHandler()
        {
            if(SelectedTermItems.Count > 0)
            {
                DialogResult ok = MessageBox.Show("Do you want to remove the selected terms?","Keyphrase Extraction", MessageBoxButtons.OKCancel);
                if(ok==DialogResult.OK)
                {
                    foreach (CandidateTerm term in SelectedTermItems)
                    {  
                        if (ControlledChecked)
                            CurrentDocument.CandidateControlledTerms.Remove(term);
                        else if (UncontrolledChecked)
                            CurrentDocument.CandidateUncontrolledTerms.Remove(term);
                    }
                    CandidateTerms.Clear();
                    CandidateTerms = ControlledChecked ? new ObservableCollection<CandidateTerm>(CurrentDocument.CandidateControlledTerms) 
                                                        : new ObservableCollection<CandidateTerm>(CurrentDocument.CandidateUncontrolledTerms);
                }
            }
            
        }

        private void DeleteKeyphrasesCommandHandler()
        {
            if (SelectedKeyphraseItems.Count > 0)
            {
                DialogResult ok = MessageBox.Show("Do you want to remove the selected keyphrases?", "Keyphrase Extraction", MessageBoxButtons.OKCancel);
                if (ok == DialogResult.OK)
                {
                    List<CandidateTerm> terms = new List<CandidateTerm>();
                    foreach (CandidateTerm term in SelectedKeyphraseItems)
                    {
                        terms.Add(term);
                    }
                    foreach (CandidateTerm term in terms)
                    {
                        CurrentDocument.RemoveAssignedKeyphrases(term);
                    }
                }
            }

        }
        
        private void ResetCommandHandler()
        {
            CurrentDocument = new Document(Path.Combine(_folderPath, _selectedFile), "pdf", "paper");
        }

        private void CloseHandler()
        {
            NLTKLibPythonProcess.Instance().StopProcess();
            _keyphraseExtractionView.Close();            
        }

        #endregion

        #region Method

        private void OpenFolderBrowserDialog()
        {
            // Configure open file dialog box
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "";
            dlg.SelectedPath = FolderPath;
            
            // Show open file dialog box
            DialogResult result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == DialogResult.OK)
            {
                // Open document 
                FolderPath = dlg.SelectedPath;
            }
        }


        private List<Tuple<CandidateTerm,double>> FilterCanTermByMachineLearning()
        {
            List<Tuple<CandidateTerm, double>> filteredTerms = new List<Tuple<CandidateTerm, double>>();
            int y = 0, n = 0;
            List<double> sample = new List<double>();
            Dictionary<string, double> score = new Dictionary<string, double>();
            double generalProbability = 0;

            DataTable ProbabilityTable = DocumentExtractionDB.Instance().GetProbabilityDataTable(ref y, ref n);
            
            foreach(CandidateTerm term in CurrentDocument.CandidateControlledTerms)            
            {
                score.Clear();
                sample.Clear();

                sample.Add(term.TfIdf);
                sample.Add(term.FirstOccurencePosition);
                sample.Add(term.OccurencePositionWeight);
                sample.Add(term.LenghtWord);
                sample.Add(term.NodeDegree);
                
                NaiveBayesClassifier.Instance().Classify(ProbabilityTable, sample, score, (double)y/(double)(y+n));
                generalProbability = score["IsKeyphrase"] / (score["IsKeyphrase"] + score["IsNotKeyphrase"]);
                term.ProbabilityRate = generalProbability;
                filteredTerms.Add(new Tuple<CandidateTerm, double>(term, generalProbability));                
            }
            return filteredTerms.OrderBy(x=>x.Item2).ToList();
        }

        private Dictionary<int, Class> FilterCanTermByPotentialClasses(List<Tuple<CandidateTerm, double>> filteredTerms)
        {
            Dictionary<int, Class> dicKeyphrases = new Dictionary<int, Class>();

            List<Class> classRelation = new List<Class>();

            foreach (Tuple<CandidateTerm,double> k in filteredTerms)
            {
                k.Item1.Classes = OntologyDB.Instance().GetRKCByIdKeyphrase(k.Item1.IdKeyphrase);
                foreach(string idClass in k.Item1.Classes)
                {
                    Class cl = classRelation.FirstOrDefault(x=>x.Id==idClass);
                    if (cl == null)
                    {
                        cl = new Class(idClass);
                        classRelation.Add(cl);
                    }
                    if (!cl.Terms.Contains(k.Item1))
                    {
                        cl.Terms.Add(k.Item1);                        
                        cl.ProbabilityRateTotal += k.Item2;
                    }
                }
            }
            classRelation = classRelation.OrderByDescending(x => x.ProbabilityRateTotal * x.Terms.Count).ToList();
            dicKeyphrases.Add(1, classRelation[0]);
            dicKeyphrases.Add(2, classRelation[1]);
            dicKeyphrases.Add(3, classRelation[2]);
            
            return dicKeyphrases;
        }


        #endregion

        //private void updateStemmedKeyPhrase()
        //{
        //    ArrayList arrKey = DBUtilities.ExecuteReaderQuery("select * from keyphrase");
        //    PorterStemmer stem = new PorterStemmer();
        //    foreach (Dictionary<string, string> k in arrKey)
        //    {
        //        string value = k.FirstOrDefault(x => x.Key == "keyphrase").Value.ToLower();
        //        string stemValue = stem.stemTerm(value).ToLower();

        //        string id = k.FirstOrDefault(x => x.Key == "id_keyphrase").Value;
        //        DBUtilities.DUpdate("stemmed_keyphrase", "keyphrase", "id_keyphrase=" + id, "'" + stemValue + "'");
                
        //    }
        //}
    }
}
