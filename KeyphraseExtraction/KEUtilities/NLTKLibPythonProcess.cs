using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.KEUtilities
{
    internal class NLTKLibPythonProcess
    {
        private static NLTKLibPythonProcess _instance;

        public static NLTKLibPythonProcess Instance()
        {
            if (_instance == null)
            {
                _instance = new NLTKLibPythonProcess();
            }            
            return _instance;
        }

        private Process process = new Process();
        private ProcessStartInfo startInfo = new ProcessStartInfo();
        public string CurrentPathFile = string.Empty;
        ScriptEngine engine;
        ScriptScope scope;
        public NLTKLibPythonProcess()
        {
        }

        public void StartProcess(string pathScriptFile)
        {
            if (_instance.IsRunning(process) && pathScriptFile != _instance.CurrentPathFile)
            {
                StopProcess();
            }

            // ref: http://stackoverflow.com/questions/13231913/how-do-i-call-a-specific-method-from-a-python-script-in-c
            engine = IronPython.Hosting.Python.CreateEngine();
            scope = engine.CreateScope();

            // ref: http://stackoverflow.com/questions/30234756/execute-python-3-code-from-c-sharp-forms-application
            // ref: http://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
            // ref: http://stackoverflow.com/questions/1704791/is-my-process-waiting-for-input
            // ref: http://stackoverflow.com/questions/5242487/input-username-to-cmd-process


            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.FileName = "python";
            startInfo.Arguments = pathScriptFile;

            process.StartInfo = startInfo;
            process.Start();
            _instance.CurrentPathFile = pathScriptFile;
        }

        public void StopProcess()
        {
            if (IsRunning(process))
            {
                process.StandardInput.WriteLine("Stop");
                process.WaitForExit();
            }
        }

        public List<string> SegmentSentences(string paragraph)
        {
            List<string> sentences = new List<string>();
            if (_instance.IsRunning(process))
            {
                process.StandardInput.WriteLine("SegmentSentences:" + paragraph);
                var listSentences = GetProcessResult("Output:");
                foreach (string sent in listSentences)
                {
                    sentences.Add(sent);
                }
            }
            return sentences;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sentence"></param>
        /// <param name="isFilterStopword"></param>
        /// <returns></returns>
        public List<string> GetNPChunkAndPoSTag(string sentence, out List<Tuple<string, string>> poSTag)
        {
            List<string> chunks = new List<string>();
            poSTag = new List<Tuple<string, string>>();
            if (_instance.IsRunning(process))
            {
                process.StandardInput.WriteLine("NPchunking:" + sentence);
                var listChunks = GetProcessResult("Output:");
                foreach (string chunk in listChunks)
                {
                    string stopword = string.Empty;
                    string filterChunk = chunk.ToLower();
                    // remove stopword out of chunk
                    if (StopWordsHandler.Instance().ContainStopword(filterChunk, out stopword))
                    {
                        filterChunk = filterChunk.Replace(stopword, "").Trim();
                    }

                    chunks.Add(filterChunk);
                }
                var posTags = GetProcessResult("PoS Tags:");
                foreach (IronPython.Runtime.PythonTuple tag in posTags)
                {
                    Tuple<string, string> poSt = new Tuple<string, string>(tag[0] as string, tag[1] as string);
                    poSTag.Add(poSt);
                }
            }
            return chunks;
        }

        public List<string> GetNGram(string sentence, int number)
        {
            List<string> grams = new List<string>();
            if (_instance.IsRunning(process))
            {
                process.StandardInput.WriteLine("NGram_" +number+":" + sentence);
                var listGrams = GetProcessResult("Output:");
                foreach (string gram in listGrams)
                {
                    if (StringProcessor.IsValidTerm(gram))
                    {
                        grams.Add(gram.ToLower().Trim());
                    }
                }
            }
            return grams;
        }

        private dynamic GetProcessResult(string resultName)
        {
            string line = null;
            bool foundOutput = false;

            do
            {
                line = process.StandardOutput.ReadLine();
                if (line.StartsWith(resultName))
                {
                    foundOutput = true;
                    line = line.Substring(resultName.Length).Trim();
                }
            }
            while (!foundOutput);

            string szCanTermVar = "lst_result";
            var pySrc = szCanTermVar + " = " + line;
            engine.Execute(pySrc, scope);

            var listGrams = scope.GetVariable(szCanTermVar);
            return listGrams;
        }

        public bool IsRunning(Process process)
        {
            try { Process.GetProcessById(process.Id); }
            catch (InvalidOperationException) { return false; }
            catch (ArgumentException) { return false; }
            return true;
        }
    }
}
