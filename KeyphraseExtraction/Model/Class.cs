using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyphraseExtraction.Model
{
    public class Class
    {
        private string _id;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<CandidateTerm> _terms;

        public List<CandidateTerm> Terms
        {
            get { return _terms; }
            set { _terms = value; }
        }

        private double _probabilityRateTotal = 0;

        public double ProbabilityRateTotal
        {
            get { return _probabilityRateTotal; }
            set { _probabilityRateTotal = value; }
        }

        //public Class(string name) 
        //{
        //    Name = name; 
        //    Terms = new List<CandidateTerm>(); 
        //}

        public Class(string id)
        {
            Id = id;
            Name = OntologyDB.Instance().GetClassNameById(id);
            Terms = new List<CandidateTerm>();
        }

    }
}
