using KeyphraseExtraction.BaseClass;
using KeyphraseExtraction.Model;
using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyphraseExtraction.ViewModel
{
    public class TrainingViewModel : ObservableNotifyObject
    {
        #region fields
        
        TrainingWindow _trainingView;
        RelayCommands _loadDocsCommand;
        RelayCommands _trainingCommand;
        RelayCommands _extractionCommand;
        RelayCommands _closeCommand;
        private ObservableCollection<DataGridColumn> _columnCollection = new ObservableCollection<DataGridColumn>();
        private ObservableCollection<DataGridColumn> _decisionColumnCollection = new ObservableCollection<DataGridColumn>();
        private string _filePath = string.Empty;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                base.RaisePropertyChanged("FilePath");
            }
        }

        private List<string> fileList = new List<string>();
        public List<string> FileList
        {
            get {
                return fileList;
            }
            set {
                fileList = value;
                base.RaisePropertyChanged("FileList");
            }
        }

        #endregion

        #region constructor
        public TrainingViewModel(TrainingWindow view)
        {
            _trainingView = view;            
            view.DataContext = this;
        }

        #endregion
        #region properties
        //public ObservableCollection<DataGridColumn> ColumnCollection
        //{
        //    get
        //    {
        //        return this._columnCollection;
        //    }
        //    set
        //    {
        //        _columnCollection = value;
        //        base.RaisePropertyChanged("ColumnCollection");
        //    }
        //}

        //public ObservableCollection<DataGridColumn> DecisionColumnCollection
        //{
        //    get
        //    {
        //        return this._decisionColumnCollection;
        //    }
        //    set
        //    {
        //        _decisionColumnCollection = value;
        //        base.RaisePropertyChanged("DecisionColumnCollection");
        //    }
        //}
        //public TrainingDataSetModel TrainingDataSet
        //{
        //    get { return _trainingDataSet; }
        //    set { 
        //        _trainingDataSet = value;
        //        base.RaisePropertyChanged("TrainingDataSet");
        //    }
        //}

        #endregion
        #region Commands
        public ICommand OpenDialogCommand
        {
            get
            {
                return _loadDocsCommand ?? (_loadDocsCommand = new RelayCommands(() => OpenDialogHandler()));
            }
        }

        public ICommand ExtractionCommand
        {
            get
            {
                return _extractionCommand ?? (_extractionCommand = new RelayCommands(() => ExtractionHandler()));
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
        
        #endregion
        #region Event handler
        private void OpenDialogHandler()
        {
            ChooseDataFilePath();
        }

        private void ExtractionHandler()
        {
            //string fileName = ChooseDataFilePath();

            //step 1: Convert the document's format to TEXT
            //string content = StringUtilities.ParseUsingPDFBox(fileName);
            //string content1 = "The OWL Web ontology language is a new formal language for representing ontologies in a Semantic web";
            //string content2 = "Web ontology language";
            //step 2: confirm the form of document (such as paper, ebooks, slide, …) => don't need
            //Document doc = new Document(fileName, content);
            //doc.ExtractCandidateKeyphrase();    

        }

        private void TrainingHandler()
        {
            _trainingView = new TrainingWindow();
            _trainingView.ShowDialog();
        }

        private void CloseHandler()
        {
            _trainingView.Close();
        }

        #endregion

        #region Method

        private string ChooseDataFilePath()
        {
            string filename = "";
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".pdf"; // Default file extension
            dlg.Filter = "PDF File (*.pdf)|*.pdf"; // Filter files by extension 
            
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                FilePath = dlg.FileName;
            }
            return filename;
        }

        
        #endregion
    }
}
