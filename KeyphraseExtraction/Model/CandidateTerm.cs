using KeyphraseExtraction.BaseClass;
using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class CandidateTerm : ObservableNotifyObject
    {
        private string _id = string.Empty;

        private string _terms = string.Empty;
        private string _stemmedTerm;

        private bool _controlledTerm = false;
        private bool _isKeyphrase;
        private int _occurenceNumber;

        private double _tf;
        private double _idf;
        private double _firstOccurencePosition;
        private double _occurencePositionWeight;
        private int _lenghtWord;
        private double _nodeDegree;
        private double _probabilityRate;      
        private string _poSTagString;

        private List<string> _classes;
        private string _idKeyphrase = string.Empty;

        public List<string> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }

        public string IdKeyphrase
        {
            get { return _idKeyphrase; }
            set { _idKeyphrase = value; }
        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
   
        public string Terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        public string StemmedTerm
        {
            get { return _stemmedTerm; }
            set { _stemmedTerm = value; }
        }

        public bool ControlledTerm
        {
            get { return _controlledTerm; }
            set { _controlledTerm = value; }
        }
        
        public bool IsKeyphrase
        {
            get { return _isKeyphrase; }
            set { _isKeyphrase = value;
                base.RaisePropertyChanged("IsKeyphrase");
            }
        }
        public int OccurenceNumber
        {
            get { return _occurenceNumber; }
            set { _occurenceNumber = value; }
        }
        
        public double Tf
        {
            get { return _tf; }
            set { _tf = value; }
        }

        public double Idf
        {
            get { return _idf; }
            set { _idf = value; }
        }

        public double TfIdf
        {
            get { return _tf * _idf; }            
        }
        
        public double FirstOccurencePosition
        {
            get { return _firstOccurencePosition; }
            set { _firstOccurencePosition = value; }
        }

        public int LenghtWord
        {
            get { return _lenghtWord; }
            set { _lenghtWord = value; }
        }

        public double NodeDegree
        {
            get { return _nodeDegree; }
            set { _nodeDegree = value; }
        }

        public double OccurencePositionWeight
        {
            get { return _occurencePositionWeight; }
            set { _occurencePositionWeight = value; }
        }
        
        public string PoSString
        {
            get { return _poSTagString; }
            set { _poSTagString = value; }
        }

        public double ProbabilityRate
        {
            get { return _probabilityRate; }
            set { _probabilityRate = value; }
        }

        private List<DocumentItem> _docItemOwners;

        public List<DocumentItem> DocItemOwners
        {
            get { return _docItemOwners; }
            set { _docItemOwners = value; }
        }

        public CandidateTerm(string id, string term, string stemmedTerm, bool isControlledTerm, double tf, double idf, double firstOccurencePosition, int lengthTerm, double nodeDegree ,double occurencePositionWeight)
        {
            Id = id;
            Terms = term;
            StemmedTerm = stemmedTerm;
            ControlledTerm = isControlledTerm;
            NodeDegree = nodeDegree;
            Tf = tf;
            Idf = idf;
            FirstOccurencePosition = firstOccurencePosition;
            LenghtWord = lengthTerm;
            OccurencePositionWeight = occurencePositionWeight;
            DocItemOwners = new List<DocumentItem>();
        }
    }
}
