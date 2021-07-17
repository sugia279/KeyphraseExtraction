using KeyphraseExtraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class DocumentStructure : ObservableNotifyObject
    {
        private DocumentItem _selectedDocItem = new DocumentItem();
        private List<DocumentItem> _docItems = new List<DocumentItem>();
        private string _errorMessage = string.Empty;

        public List<DocumentItem> DocumentItems
        {
            get { return _docItems; }
            set { 
                _docItems = value;
                RaisePropertyChanged(() => DocumentItems);
            }
        }

        public DocumentItem SelectedDocItem
        {
            get { return _selectedDocItem; }
            set
            {
                _selectedDocItem = value;
                RaisePropertyChanged(() => SelectedDocItem);
            }
        }
        
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            {
                _errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            }
        }
        public DocumentStructure()
        { 
        
        }

        public List<DocumentItem> LoadStructure()
        {
            _docItems.Add(new DocumentItem("Title", -1, -1, 0.9, string.Empty));
            _docItems.Add(new DocumentItem("Abstract", -1, -1, 0.7, string.Empty));
            _docItems.Add(new DocumentItem("Keywords", -1, -1, 1, string.Empty));
            _docItems.Add(new DocumentItem("Introduction", -1, -1, 0.6, string.Empty));
            _docItems.Add(new DocumentItem("Conclusions", -1, -1, 0.7, string.Empty));
            _docItems.Add(new DocumentItem("References", -1, -1, 0.7, string.Empty));            
            _docItems.Add(new DocumentItem("Unspecified", -1, -1, 0.5, string.Empty));
            RaisePropertyChanged(() => DocumentItems);
            
            return _docItems;
        }
         
        internal bool IsValidStructure(int totalRawTextline)
        {
            bool isValid = true;
            foreach (DocumentItem item in DocumentItems)
            {
                if (item.BeginRow == -1 && item.EndRow == -1)
                {
                    continue;
                }
                else if ((item.BeginRow == -1 && item.EndRow > -1) 
                            || (item.BeginRow > -1 && item.EndRow == -1)
                            || (item.EndRow < item.BeginRow))
                {
                    ErrorMessage = string.Format("The begin and end row number of \"{0}\" item are not specified incorrectly.", item.ItemName);
                    isValid = false;
                    break;
                }
                else if ((item.BeginRow > totalRawTextline) || (item.EndRow > totalRawTextline))
                {
                    ErrorMessage = string.Format("The begin or end row number of \"{0}\" item is out of paper content range.", item.ItemName);
                    isValid = false;
                    break;
                }

                DocumentItem docItem = _docItems.FirstOrDefault(x => x.BeginRow <= item.BeginRow
                                                            && x.EndRow >= item.BeginRow
                                                            && x.ItemName != item.ItemName);
                DocumentItem docItem1 = _docItems.FirstOrDefault(x => x.BeginRow <= item.EndRow
                                                                && x.EndRow >= item.EndRow
                                                                && x.ItemName != item.ItemName);
                if (docItem != null || docItem1 != null)
                {
                    ErrorMessage = string.Format("The \"{0}\" begin row number is overlaped in another item", SelectedDocItem.ItemName);
                    isValid = false;
                    break;
                }
                ErrorMessage = string.Empty;
            }
            return isValid;
        }
    }
}
