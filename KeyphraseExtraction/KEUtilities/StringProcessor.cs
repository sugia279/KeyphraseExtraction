using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KeyphraseExtraction.KEUtilities
{
    /// <summary>
    /// Contains methods for counting words.
    /// </summary>
    public static class StringProcessor
    {
        /// <summary>
        /// Count words with Regex.
        /// </summary>
        public static int CountWords1(string s)
        {
            MatchCollection collection = Regex.Matches(s, @"[\S]+");
            return collection.Count;
        }
        
        public static bool IsValidTerm(string term)
        {
            bool isValid = true;
            var specialChars = new[] { '\\', '/', ':', '*', '<', '>', '|', '#', '{', '}', '%', '~', '&', '(', ')' };
            string stopword = string.Empty;
            if (!term.Any(c => Char.IsLetterOrDigit(c)))
            {
                isValid = false;
            }
            else if (StopWordsHandler.Instance().ContainStopword(term, out stopword))
            {
                isValid = false;
            }
            else if (term.Any(c => Char.IsPunctuation(c)))
            {
                isValid = false;
            }
            else if (specialChars.Where(term.Contains).Count() > 0)
            {
                isValid = false;
            }
            return isValid;
        }
        /// <summary>
        /// Count word with loop and character tests.
        /// </summary>
        public static int CountWords2(string s)
        {
            int c = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i - 1]) == true)
                {
                    if (char.IsLetterOrDigit(s[i]) == true ||
                        char.IsPunctuation(s[i]))
                    {
                        c++;
                    }
                }
            }
            if (s.Length > 2)
            {
                c++;
            }
            return c;
        }
    }
}
