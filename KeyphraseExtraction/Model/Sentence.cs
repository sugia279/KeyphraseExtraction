using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class Sentence
    {
        private string _content;
        private int _wordLenght=0;

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        
        public int WordLenght
        {
            get { return _wordLenght; }
            set { _wordLenght = value; }
        }
        private DocumentItem _docItemOwner;

        public DocumentItem DocItemOwner
        {
            get { return _docItemOwner; }
            set { _docItemOwner = value; }
        }

        public Sentence(string content)
        {
            _content = content;
            _wordLenght = _content.Split(' ').Count();
            _docItemOwner = new DocumentItem();
        }
        public Sentence(string content, DocumentItem item)
        {
            _content = content;

            _docItemOwner = item;
        }

        public List<string> ExtractCandidateTerm()
        {
            List<string> canTerms = new List<string>();
            
            // extract ngram
            canTerms = NLTKLibPythonProcess.Instance().GetNGram(Content, 5);

            List<Tuple<string, string>> poSTags;
            // extract np chunk & pos patterns
            List<string> chunks = NLTKLibPythonProcess.Instance().GetNPChunkAndPoSTag(Content, out poSTags);

            canTerms.AddRange(chunks.Where(chunk => !canTerms.Contains(chunk)).ToList());

            RemoveSubsumedTerms(ref canTerms);
            
            return canTerms;
        }

        private void RemoveSubsumedTerms(ref List<string> canTerms)
        {
            List<string> subsumedTerm = new List<string>();

            for (int i = 0; i < canTerms.Count; i++)
            {
                int j = i + 1;
                int wordLength = StringProcessor.CountWords1(canTerms[i]);
                for (; j < canTerms.Count; j++)
                {
                    if (StringProcessor.CountWords1(canTerms[j]) > wordLength
                        && canTerms[j].Contains(canTerms[i]))
                    {
                        subsumedTerm.Add(canTerms[i]);
                        break;
                    }
                }
            }

            foreach (string term in subsumedTerm)
            {
                canTerms.Remove(term);
            }
        }
    }
}
