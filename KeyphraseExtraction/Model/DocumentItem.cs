using KeyphraseExtraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class DocumentItem : ObservableNotifyObject
    {
        private string _itemName = null;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value;
            RaisePropertyChanged(() => ItemName);
            }
        }
        private int _beginRow = -1;

        public int BeginRow
        {
            get { return _beginRow; }
            set
            {                
                if (value >= 1)
                {
                    _beginRow = value;                    
                }
                RaisePropertyChanged(() => BeginRow);                
            }
        }
        private int _endRow = -1;

        public int EndRow
        {
            get { return _endRow; }
            set
            {
                if (value >= BeginRow && value >= 0)
                {
                    _endRow = value;                    
                }
                RaisePropertyChanged(() => EndRow);               
            }
        }

        private double _weight = -1;

        public double Weight
        {
            get { return _weight; }
            set { _weight = value;
            RaisePropertyChanged(() => Weight);
            }
        }      

        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value;
            RaisePropertyChanged(() => Description);
            }
        }

        public List<CandidateTerm> CandidateTerms
        {
            get;
            set;
        }
        public DocumentItem(string item, int beginRow, int endRow, double weight,string description)
        {
            ItemName = item;
            BeginRow = beginRow;
            EndRow = endRow;
            Weight = weight;
            Description = description;
            CandidateTerms = new List<CandidateTerm>();
        }

        public DocumentItem()
        {   
        }

        public DocumentItem(DocumentItem item)
        {
            ItemName = item.ItemName;
            BeginRow = item.BeginRow;
            EndRow = item.EndRow;
            Weight = item.Weight;
            Description = item.Description;
            CandidateTerms = new List<CandidateTerm>();
        }
    }
}
