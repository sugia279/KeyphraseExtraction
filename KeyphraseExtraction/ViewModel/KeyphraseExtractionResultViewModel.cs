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
    public class KeyphraseExtractionResultViewModel : ObservableNotifyObject
    {
        #region fields
        KeyphraseExtractionResultWindow _keyphraseExtractionResultView;
        RelayCommands _extractionCommand;
        RelayCommands _comfirmCommand;
        RelayCommands _closeCommand;


        private List<CandidateTerm> _keyphrasesBy1Class = new List<CandidateTerm>();

        public List<CandidateTerm> KeyphrasesBy1Class
        {
            get { return _keyphrasesBy1Class; }
            set { _keyphrasesBy1Class = value;
            RaisePropertyChanged(() => KeyphrasesBy1Class);
            }
        }
        private List<CandidateTerm> _keyphrasesBy2Classes = new List<CandidateTerm>();

        public List<CandidateTerm> KeyphrasesBy2Classes
        {
            get { return _keyphrasesBy2Classes; }
            set { _keyphrasesBy2Classes = value;
            RaisePropertyChanged(() => KeyphrasesBy2Classes);
            }
        }
        private List<CandidateTerm> _keyphrasesBy3Classes = new List<CandidateTerm>();

        public List<CandidateTerm> KeyphrasesBy3Classes
        {
            get { return _keyphrasesBy3Classes; }
            set { _keyphrasesBy3Classes = value;
            RaisePropertyChanged(() => KeyphrasesBy3Classes);
            }
        }


        private List<CandidateTerm> _keyphrasesByKeywords = new List<CandidateTerm>();

        public List<CandidateTerm> AssignedKeyphrases
        {
            get { return _keyphrasesByKeywords; }
            set
            {
                _keyphrasesByKeywords = value;
                RaisePropertyChanged(() => _keyphrasesByKeywords);
            }
        }

        public List<CandidateTerm> SelectedKeyphrases
        {
            get;
            set;
        }
        private bool _oneClassChecked = true;
        public bool OneClassChecked {
            get { return _oneClassChecked; }
            set { _oneClassChecked = value; }
        }

        private bool _twoClassesChecked = true;
        public bool TwoClassesChecked
        {
            get { return _twoClassesChecked; }
            set { _twoClassesChecked = value; }
        }

        private bool _threeClassesChecked = true;
        public bool ThreeClassesChecked
        {
            get { return _threeClassesChecked; }
            set { _threeClassesChecked = value; }
        }

        private bool _keywordsSectionChecked;

        public bool AssignedKeyphrasesChecked
        {
            get { return _keywordsSectionChecked; }
            set
            {
                _keywordsSectionChecked = value;
                RaisePropertyChanged(() => AssignedKeyphrasesChecked);
            }
        }


        private string _oneClassName;

        public string OneClassName
        {
            get { return _oneClassName; }
            set { _oneClassName = value;
            RaisePropertyChanged(() => OneClassName);
            }
        }

        private string _twoClassesName;

        public string TwoClassesName
        {
            get { return _twoClassesName; }
            set { _twoClassesName = value;
            RaisePropertyChanged(() => TwoClassesName);
            }
        }

        private string _threeClassesName;

        public string ThreeClassesName
        {
            get { return _threeClassesName; }
            set { _threeClassesName = value;
            RaisePropertyChanged(() => ThreeClassesName);
            }
        }

        private int _specifiedKeyphraseNumber;

        public int SpecifiedKeyphraseNumber
        {
            get { return _specifiedKeyphraseNumber; }
            set 
            {
                _specifiedKeyphraseNumber = value;
                RaisePropertyChanged(() => SpecifiedKeyphraseNumber);
            }
        }

        private int _matchedKeyphraseBy1ClassNumber = 0;

        public int MatchedKeyphraseBy1ClassNumber
        {
            get { return _matchedKeyphraseBy1ClassNumber; }
            set { _matchedKeyphraseBy1ClassNumber = value;
            RaisePropertyChanged(() => MatchedKeyphraseBy1ClassNumber);
            }
        }

        private int _matchedKeyphraseBy2ClassesNumber = 0;

        public int MatchedKeyphraseBy2ClassesNumber
        {
            get { return _matchedKeyphraseBy2ClassesNumber; }
            set { _matchedKeyphraseBy2ClassesNumber = value;
            RaisePropertyChanged(() => MatchedKeyphraseBy2ClassesNumber);
            }
        }

        private int _matchedKeyphraseBy3ClassesNumber = 0;

        public int MatchedKeyphraseBy3ClassesNumber
        {
            get { return _matchedKeyphraseBy3ClassesNumber; }
            set { _matchedKeyphraseBy3ClassesNumber = value;
            RaisePropertyChanged(() => MatchedKeyphraseBy3ClassesNumber);
            }
        }
        private Dictionary<int, Class> _extractClasses = new Dictionary<int,Class>();
        #endregion

        #region constructor
        public KeyphraseExtractionResultViewModel(KeyphraseExtractionResultWindow view, Dictionary<int, Class> extractedClasses, List<CandidateTerm> manualKeyphrases)
        {
            _keyphraseExtractionResultView = view;
            SpecifiedKeyphraseNumber = 20;
            _extractClasses = extractedClasses;
            AssignedKeyphrases = manualKeyphrases;
            SelectedKeyphrases = new List<CandidateTerm>();
            view.DataContext = this;            
        }

        #endregion
        #region Commands
        
        
        public ICommand ExtractionCommand
        {
            get
            {
                return _extractionCommand ?? (_extractionCommand = new RelayCommands(() => ExtractionHandler()));
            }
        }

        public ICommand ConfirmCommand
        {
            get
            {
                return _comfirmCommand ?? (_comfirmCommand = new RelayCommands(() => ConfirmHandler()));
            }
        }


        public RelayCommands CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommands(() => CloseHandler()));
            }
        }

        #endregion

        #region Event handler

        private void ExtractionHandler()
        {
            string classname = string.Empty;
            KeyphrasesBy1Class = GetTermsByNClasses(SpecifiedKeyphraseNumber, 1, _extractClasses,ref  classname);
            OneClassName = classname;
            MatchedKeyphraseBy1ClassNumber = GetMatchedAutoKeyphraseNumber(KeyphrasesBy1Class,AssignedKeyphrases);
            KeyphrasesBy2Classes = GetTermsByNClasses(SpecifiedKeyphraseNumber, 2, _extractClasses, ref  classname);
            TwoClassesName = classname;
            MatchedKeyphraseBy2ClassesNumber = GetMatchedAutoKeyphraseNumber(KeyphrasesBy2Classes, AssignedKeyphrases);
            KeyphrasesBy3Classes = GetTermsByNClasses(SpecifiedKeyphraseNumber, 3, _extractClasses, ref  classname);
            ThreeClassesName = classname;
            MatchedKeyphraseBy3ClassesNumber = GetMatchedAutoKeyphraseNumber(KeyphrasesBy3Classes, AssignedKeyphrases); ;

        }

        private void ConfirmHandler()
        {
            SelectedKeyphrases.Clear();
            if (OneClassChecked)
            {   
                AddingSelectedKeyphrases(KeyphrasesBy1Class);
            }
            if (TwoClassesChecked)
            {
                AddingSelectedKeyphrases(KeyphrasesBy2Classes);
            }
            if (ThreeClassesChecked)
            {
                AddingSelectedKeyphrases(KeyphrasesBy3Classes);
            }

            if (AssignedKeyphrasesChecked)
            {
                AddingSelectedKeyphrases(AssignedKeyphrases);
            }
            
            _keyphraseExtractionResultView.Close();  
        }

        private void AddingSelectedKeyphrases(List<CandidateTerm> terms)
        {
            foreach (CandidateTerm t in terms)
            {
                if (SelectedKeyphrases.Contains(t) == false)
                {
                    SelectedKeyphrases.Add(t);
                }
            }
        }

        private void CloseHandler()
        {
            _keyphraseExtractionResultView.Close();  
        }
        #endregion

        #region Methods
        private List<CandidateTerm> GetTermsByNClasses(int specifiedKeyphraseNumber, int classesExtractionNumber, Dictionary<int, Class> extractedClasses,ref string className)
        {
            List<CandidateTerm> terms = new List<CandidateTerm>();
            int totalTermInClasses = 0;
            className = string.Empty;
            for (int i = 0; i < classesExtractionNumber; i++)
            {
                totalTermInClasses += extractedClasses[i+1].Terms.Count;
                className += extractedClasses[i + 1].Name + ",";
            }
            className = (className.LastIndexOf(",") == className.Length - 1) ? className.Remove(className.Length - 1) : className;

            double n = 0;
            for (int i = 0; i < classesExtractionNumber; i++)
            {
                n = Math.Round(((double)extractedClasses[i+1].Terms.Count / (double)totalTermInClasses) * (double)specifiedKeyphraseNumber);
                for (int j = 0; j < n; j++)
                {
                    if (j < extractedClasses[i + 1].Terms.Count && !terms.Contains(extractedClasses[i + 1].Terms[j]))
                    {
                        terms.Add(extractedClasses[i + 1].Terms[j]);
                    }
                }
            }

            return terms;
        }

        private int GetMatchedAutoKeyphraseNumber(List<CandidateTerm> AutoKeyphraseExtractionList, List<CandidateTerm> ManualKeyphraseExtractionList)
        {
            int n = 0;
            foreach (CandidateTerm term in AutoKeyphraseExtractionList)
            {
                if (ManualKeyphraseExtractionList.FirstOrDefault(x => x.StemmedTerm == term.StemmedTerm)!=null)
                {
                    n++;
                }
            }
            return n;
        }
        #endregion
    }
}
