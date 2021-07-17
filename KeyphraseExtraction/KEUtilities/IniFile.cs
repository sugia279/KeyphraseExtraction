using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using LogiGear.TestArchitect.VisualStudio.Common.Model.Constants;

namespace KeyphraseExtraction.KEUtilities
{
    public class IniFile
    {
        private static string DefaultSection = "_[Default section for pairs key/value without specific section]_";

        private static IniFile _instance;
        private Dictionary<string, string> _entries;
        private Dictionary<string, List<string>> _sections;
        private string _filePath = "";

        /// <summary>
        /// Ini file's path
        /// </summary>
        public string FilePath
        {
            get { return _filePath; }
        }

        /// <summary>
        /// get permission of ini file
        /// </summary>
        private bool _bPermission = true;
        public bool Permission
        {
            get 
            {
                return _bPermission;
            }
        }

        private IniFile()
        {
            //object path = Registry.GetValue(TAConst.TARootKey, TAConst.FileIniPathKeyValue, null);
            //if (System.Environment.Is64BitOperatingSystem)
            //    path = Registry.GetValue(TAConst.TARootKey64, TAConst.FileIniPathKeyValue, null);

            _entries = new Dictionary<string, string>();
            _sections = new Dictionary<string, List<string>>();
            _sections.Add(DefaultSection, new List<string>());
            if (path != null && path.ToString().Trim().Length > 0)
            {
                string sPath = path.ToString().Trim();
                _filePath = sPath + (sPath.EndsWith(TAConst.PathSeperator) ? "" : TAConst.PathSeperator) + TAConst.FileIniName;
            }
        }

        private void ReadContent()
        {
            // check permission for ini file
            if (FileUtility.CheckPermissionOnFile(_filePath, enuPermissionType.Read) == false)
            {
                _bPermission = false;
            }
            else
            {
                ReadFile();
                _bPermission = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void ReadFile()
        {   
            if (File.Exists(_filePath) == false)
                return;

            using (StreamReader fs = new StreamReader(_filePath))
            {
                string line = fs.ReadLine(), key, val, currentSection = "";
                List<string> keysInSection = null;
                int idx;

                while (string.IsNullOrEmpty(line) == false)
                {
                    if (IsSectionLine(line, ref currentSection) == false)
                    {
                        idx = line.IndexOf("=");
                        if (idx > 0)
                        {
                            key = line.Substring(0, idx).Trim().ToLower();
                            val = line.Substring(idx + 1).Trim();

                            if (currentSection.Length > 0)
                            {
                                _sections.TryGetValue(currentSection, out keysInSection);
                                if (keysInSection.Contains(key) == false)
                                    keysInSection.Add(key);
                            }

                            if (_entries.ContainsKey(key) == false)
                                _entries.Add(key, val);
                            else if (currentSection.Length > 0)
                            {
                                _entries.Add(key + "_[" + currentSection.ToLower() + "]^|^", val);
                                keysInSection.Remove(key);
                                keysInSection.Add(key + "_[" + currentSection.ToLower() + "]^|^");
                            }

                        }
                    }
                    else if (_sections.ContainsKey(currentSection) == false)
                        _sections.Add(currentSection, new List<string>());

                    line = fs.ReadLine();

                }

                fs.Close();
            }
        }

        /// <summary>
        /// Check a line is a section or not
        /// </summary>
        /// <param name="line"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        private bool IsSectionLine(string line, ref string sectionName)
        {
            bool result = false;

            sectionName = string.IsNullOrEmpty(sectionName) ? DefaultSection : sectionName;
            if (string.IsNullOrEmpty(line) == false)
            {
                line = line.Trim();
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    sectionName = line.Substring(1, line.Length - 2);
                    result = true;
                }
            }

            return result;
        }

        public static IniFile Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new IniFile();
                    _instance.ReadContent();
                }

                return _instance;
            }
        }

        public static IniFile GetInstance(string filePath)
        {
            IniFile newInstance = null;

            if (File.Exists(filePath) == true)
            {
                newInstance = new IniFile();
                newInstance._filePath = filePath;
                newInstance.ReadContent();
            }

            return newInstance;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            
            foreach (KeyValuePair<string, List<string>> pair in _sections)
            {
                if (pair.Key.Equals(DefaultSection))
                    continue;

                GetSectionContent(pair.Key, result);
            }
            GetSectionContent(DefaultSection, result);

            return result.ToString().TrimEnd();
        }

        /// <summary>
        /// Get all keys/values of a section that have name is sectionName.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="content"></param>
        private void GetSectionContent(string sectionName, StringBuilder content)
        {
            List<string> keysInSection = null;
            string value, sKey;

            if (sectionName.Equals(DefaultSection) == false)
                content.Append("[" + sectionName + "]").Append(TAConst.EndLine);

            if (_sections.TryGetValue(sectionName, out keysInSection) == true)
            {
                foreach (string key in keysInSection)
                {
                    _entries.TryGetValue(key, out value);
                    sKey = key;
                    if (key.EndsWith("^|^") == true)
                        sKey = sKey.Substring(0, sKey.LastIndexOf("_["));
                    content.Append(sKey + "=" + value).Append(TAConst.EndLine);
                }
            }
        }

        /// <summary>
        /// Get a value from its key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key) 
        {
            string result = null;

            key = key.ToLower();
            if (_entries.ContainsKey(key))
                _entries.TryGetValue(key, out result);
            else
            {
                foreach (var pair in _sections)
                {
                    if (pair.Value.IndexOf(key + "_[" + pair.Key.ToLower() + "]^|^") != -1)
                    {
                        _entries.TryGetValue(key + "_[" + pair.Key.ToLower() + "]^|^", out result);
                        break;
                    }
                }
            }

            return result;
        }

        public string GetValue(string section, string key)
        {
            string result = null;
            List<string> keys = null;

            key = key.ToLower();
            if (_sections.ContainsKey(section) == true)
            {
                _sections.TryGetValue(section, out keys);
                foreach (string key2 in keys)
                {
                    if (key2.Equals(key) == true || key2.Equals(key + "_[" + section.ToLower() + "]^|^") == true)
                    {
                        if (key2.Equals(key) == true)
                            _entries.TryGetValue(key, out result);
                        else
                            _entries.TryGetValue(key + "_[" + section.ToLower() + "]^|^", out result);

                        break;
                    }
                }
            }


            return result;
        }

        /// <summary>
        /// Set new value for a exist key or new key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, string value)
        {
            bool result = true;
            bool hasSection = false;
            
            key = key.ToLower();
            if (_entries.ContainsKey(key))
                _entries.Remove(key);

            foreach (KeyValuePair<string, List<string>> pair in _sections)
            {
                if (pair.Value.Contains(key) == true)
                {
                    hasSection = true;
                    break;
                }
            }

            if (hasSection == false)
            { 
                List<string> keys = null;
                _sections.TryGetValue(DefaultSection, out keys);
                keys.Add(key);
            }
            
            _entries.Add(key, string.IsNullOrEmpty(value) ? "" : value.Trim());            
            //++<Hien - 10/08/12> MSI-1140: Update message when TA4VS does not work properly if it does not have permission to read/write files.
            result = Save();
            //--<Hien - 10/08/12> MSI-1140
            return result;
        }

        /// <summary>
        /// Set new value for a exist key or new key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string section, string key, string value)
        {
            bool result = true;
            //bool hasSection = false;
            List<string> keys = null;

            key = key.ToLower();
            key = GetKeyInSection(section, key);

            _sections.TryGetValue(_sections.ContainsKey(section) ? section : DefaultSection, out keys);
            if (keys.Contains(key) == false)
                keys.Add(key);
            if (_entries.ContainsKey(key) == true)
                _entries.Remove(key);

            _entries.Add(key, string.IsNullOrEmpty(value) ? "" : value.Trim());
            //++<Hien - 10/08/12> MSI-1140: Update message when TA4VS does not work properly if it does not have permission to read/write files.
            result = Save();
            //--<Hien - 10/08/12> MSI-1140
            return result;
        }

        private string GetKeyInSection(string section, string key)
        {
            List<string> keys = null;
            string newKey = key;
            bool found = false;

            _sections.TryGetValue(section, out keys);
            if (keys != null)
            {
                foreach (string key2 in keys)
                {
                    if (key2.Equals(key) == true || key2.Equals(key + "_[" + section.ToLower() + "]^|^") == true)
                    {
                        if (key2.Equals(key) == true)
                            _entries.TryGetValue(key, out newKey);
                        else
                            _entries.TryGetValue(key + "_[" + section.ToLower() + "]^|^", out newKey);

                        found = true;
                        break;
                    }
                }

                if (found == false && _entries.ContainsKey(key) == true)
                    newKey = key + "_[" + section.ToLower() + "]^|^";
            }
            else
            {
                _sections.Add(section, new List<string>());
            }

            return newKey;
        }

        /// <summary>
        /// Delete a key in _entries
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string DeleteValue(string key)
        {
            string result = null;

            key = key.ToLower();
            if (_entries.ContainsKey(key))
            {
                _entries.Remove(key);
                foreach (var pair in _sections)
                {
                    if (pair.Value.Contains(key) == true)
                    {
                        pair.Value.Remove(key);
                        break;
                    }
                }
            }
            
            Save();

            return result;
        }

        /// <summary>
        /// Delete a key in _entries
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string DeleteValue(string section, string key)
        {
            string result = null;
            List<string> keys = null;

            key = key.ToLower();
            _sections.TryGetValue(section, out keys);
            if (keys != null && keys.Contains(key + "_[" + section.ToLower() + "]^|^") == true)
            {
                key = key + "_[" + section.ToLower() + "]^|^";
            }

            if (_entries.ContainsKey(key) == true)
            {
                keys.Remove(key);
                _entries.Remove(key);
            }
            
            Save();

            return result;
        }

        /// <summary>
        /// Save pairs key-value into file
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public bool Save()
        {
            // check permission write of file 
            if (FileUtility.CheckPermissionOnFile(_filePath, enuPermissionType.Write) == false)
                return false;

            FileInfo f = new FileInfo(_filePath);

            if (f.Directory.Exists == false)
                f.Directory.Create();
            using (StreamWriter writer = new StreamWriter(_filePath, false))
            {
                writer.Write(ToString().ToArray());
                writer.Close();
            }

            return true;
        }

        /// <summary>
        /// Remove all data in _entries and re-read from the file. 
        /// </summary>
        public void Reset()
        {
            _entries.Clear();
            _sections.Clear();
            _sections.Add(DefaultSection, new List<string>());
            ReadFile();
        }
    }
}
