using KeyphraseExtraction.KEUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class OntologyDB
    {
        private static OntologyDB _instance;

        public static OntologyDB Instance()
        {
            if (_instance == null)
            {
                _instance = new OntologyDB();
            }
            return _instance;
        }

        private ArrayList _keyphrases;

        public ArrayList Keyphrases
        {
            get { return _keyphrases; }
            set { _keyphrases = value; }
        }
        private ArrayList _classes;

        public ArrayList Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }
        private ArrayList _relationships;

        public ArrayList Relationships
        {
            get { return _relationships; }
            set { _relationships = value; }
        }
        private ArrayList _classClassRelationship;

        public ArrayList ClassClassRel
        {
            get { return _classClassRelationship; }
            set { _classClassRelationship = value; }
        }
        private ArrayList _keyphraseClassRelationship;

        public ArrayList KeyphraseClassRel
        {
            get { return _keyphraseClassRelationship; }
            set { _keyphraseClassRelationship = value; }
        }
        private ArrayList _keyphraseKeyphraseRelationship;

        public ArrayList KeyphraseKeyphraseRel
        {
            get { return _keyphraseKeyphraseRelationship; }
            set { _keyphraseKeyphraseRelationship = value; }
        }

        public OntologyDB()
        { }

        public void LoadDB()
        {
            DBUtilities.OpenConnection("CK_ONTO");
            Keyphrases = DBUtilities.ExecuteReaderQuery("select * from keyphrase");
            Classes = DBUtilities.ExecuteReaderQuery("select * from class");
            Relationships = DBUtilities.ExecuteReaderQuery("select * from relationship");
            ClassClassRel = DBUtilities.ExecuteReaderQuery("select * from class_class_relationship");
            KeyphraseClassRel = DBUtilities.ExecuteReaderQuery("select * from class_keyphrase_relationship");
            KeyphraseKeyphraseRel = DBUtilities.ExecuteReaderQuery("select * from keyphrase_keyphrase_relationship");
            DBUtilities.CloseConnection();
        }

        public Dictionary<string,string> GetKeyphrasesByStemmedKeyphrase(string stemmedKeyphrase)
        {
            Dictionary<string, string> kp = null;
            foreach (Dictionary<string, string> kDict in Keyphrases)
            {
                if (kDict["stemmed_keyphrase"].ToLower() == stemmedKeyphrase)
                {
                    kp = kDict;
                    break;
                }

            }
            return kp;
        }

        public double GetNodeDegreeStemmedKeyphrase(string idKeyphrase)
        {
            double nodeDegree = 0;
            var query = from Dictionary<string,string> kkr in KeyphraseKeyphraseRel 
                        from Dictionary<string,string> rel in Relationships                        
                        where kkr["id_keyphrase1"] == idKeyphrase && kkr["id_relationship"] == rel["id_relationship"] 
                        select rel["value"];

            foreach (string value in query)
            {
                nodeDegree = nodeDegree + double.Parse(value);
            }
            return Math.Round(nodeDegree,2);
        }

        public List<string> GetRKCByIdKeyphrase(string idKeyphrase)
        {            
            var query = from Dictionary<string, string> rkc in KeyphraseClassRel                        
                        where rkc["id_keyphrase"] == idKeyphrase
                        select rkc["id_class"];
            
            return query.ToList();
        }

        
        public string GetClassNameById(string idClass)
        {
            string name = string.Empty;
            var query = from Dictionary<string, string> cls in Classes
                        where cls["id_class"] == idClass
                        select cls["class"];
            return query.ToArray()[0];
        }
    }
}
