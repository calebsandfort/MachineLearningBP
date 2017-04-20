using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class DecisionNode
    {
        public int col { get; set; }
        public String value { get; set; }
        public List<TreeResult> results { get; set; }
        public DecisionNode tb { get; set; }
        public DecisionNode fb { get; set; }

        public DecisionNode()
        {
            this.col = -1;
            this.results = null;
            this.tb = null;
            this.fb = null;
        }
    }

    public class TreeResult
    {
        public int count { get; set; }
        public String value { get; set; }
    }
}
