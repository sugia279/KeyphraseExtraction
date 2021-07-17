using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace KeyphraseExtraction.KEUtilities
{
	/// <summary>
	/// Summary description for NGram.
	/// </summary>
	public class NGram
	{
		public NGram()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        //*********************************************************************************************************
        // © 2013 jakemdrew.com. All rights reserved. 
        // This source code is licensed under The GNU General Public License (GPLv3):  
        // http://opensource.org/licenses/gpl-3.0.html
        //*********************************************************************************************************

        //*********************************************************************************************************
        //makeNgrams - Example n-gram creator.
        //Created By - Jake Drew 
        //Version -    1.0, 04/22/2013
        //*********************************************************************************************************
        public IEnumerable<string> makeNgrams(string text, int nGramSize)
        {
            if (nGramSize == 0) throw new Exception("nGram size was not set");

            StringBuilder nGram = new StringBuilder();
            Queue<int> wordLengths = new Queue<int>();

            int wordCount = 0;
            int lastWordLen = 0;

            //append the first character, if valid.
            //avoids if statement for each for loop to check i==0 for before and after vars.
            if (text != "" && char.IsLetterOrDigit(text[0]))
            {
                nGram.Append(text[0]);
                lastWordLen++;
            }

            //generate ngrams
            for (int i = 1; i < text.Length - 1; i++)
            {
                char before = text[i - 1];
                char after = text[i + 1];

                if (char.IsLetterOrDigit(text[i])
                        ||
                    //keep all punctuation that is surrounded by letters or numbers on both sides.
                        (text[i] != ' '
                        && (char.IsSeparator(text[i]) || char.IsPunctuation(text[i]))
                        && (char.IsLetterOrDigit(before) && char.IsLetterOrDigit(after))
                        )
                    )
                {
                    nGram.Append(text[i]);
                    lastWordLen++;
                }
                else
                {
                    if (lastWordLen > 0)
                    {
                        wordLengths.Enqueue(lastWordLen);
                        lastWordLen = 0;
                        wordCount++;

                        if (wordCount >= nGramSize)
                        {
                            yield return nGram.ToString();
                            nGram.Remove(0, wordLengths.Dequeue() + 1);
                            wordCount -= 1;
                        }

                        nGram.Append(" ");
                    }
                }
            }
            nGram.Append(text.Last());
            yield return nGram.ToString();
        }

        //public static float ComputeNGramSimilarity(string text1, string text2, int gramlength)
        //{
        //    if ((object)text1 == null || (object)text2 == null || text1.Length == 0 || text2.Length == 0)
        //        return 0.0F;
        //    string[] grams1 = GenerateNGramsWord(text1, gramlength);
        //    string[] grams2 = GenerateNGramsWord(text2, gramlength);
        //    int count = 0;
        //    for (int i = 0; i < grams1.Length; i++)
        //    {
        //        for (int j = 0; j < grams2.Length; j++)
        //        {
        //            if (!grams1[i].Equals(grams2[j]))
        //                continue;
        //            count++;
        //            break;
        //        }
        //    }

        //    float sim = (2.0F * (float)count) / (float)(grams1.Length + grams2.Length);
        //    return sim;
        //}

        //public static float GetBigramSimilarity(string text1, string text2)
        //{
        //    return ComputeNGramSimilarity(text1, text2, 2);
        //}

        //public static float GetTrigramSimilarity(string text1, string text2)
        //{
        //    return ComputeNGramSimilarity(text1, text2, 3);
        //}

        //public static float GetQuadGramSimilarity(string text1, string text2)
        //{
        //    return ComputeNGramSimilarity(text1, text2, 4);
        //}
		
	}
}
