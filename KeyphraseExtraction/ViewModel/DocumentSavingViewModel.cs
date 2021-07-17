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

namespace KeyphraseExtraction.ViewModel
{
    public enum ButtonTypePressed
    { 
        Save,
        GenerateFile,
        Close
    }
    public class DocumentSavingViewModel : ObservableNotifyObject
    {
        #region fields
        DocumentSavingWindow _documentSavingView;
        RelayCommands _closeCommand;
        RelayCommands _continueCommand;
        RelayCommands _generateCommand;

        Document _doc;

        public Document DocInfo
        {
            get { return _doc; }
            set { _doc = value; }
        }

        public ButtonTypePressed GeneratePressed
        {
            get;
            set;
        }
        public ButtonTypePressed SaveToTrainPressed
        {
            get;
            set;
        }
        #endregion

        #region constructor
        public DocumentSavingViewModel(DocumentSavingWindow view, Document doc)
        {
            _documentSavingView = view;
            DocInfo = doc;
            GeneratePressed = ButtonTypePressed.Close;
            SaveToTrainPressed = ButtonTypePressed.Close;
            view.DataContext = this;            
        }

        #endregion
        #region Commands
        
        
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommands(() => CloseHandler()));
            }
        }


        public RelayCommands ContinueCommand
        {
            get
            {
                return _continueCommand ?? (_continueCommand = new RelayCommands(() => SaveToTrainCommandHandler()));
            }           
        }

        public RelayCommands GenerateCommand
        {
            get
            {
                return _generateCommand ?? (_generateCommand = new RelayCommands(() => GenrateCommandHandler()));
            }
        }

        #endregion

        #region Event handler

        private void CloseHandler()
        {
            _documentSavingView.Close();            
        }

        private void SaveToTrainCommandHandler()
        {
            SaveToTrainPressed = ButtonTypePressed.Save;              
        }


        private void GenrateCommandHandler()
        {
            GeneratePressed = ButtonTypePressed.GenerateFile;            
        }
        #endregion
    }
}
