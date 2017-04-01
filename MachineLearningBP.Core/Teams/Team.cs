using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Teams
{
    public class Team : Entity<int>
    {
        public String EspnName { get; set; }
        public String CoversName { get; set; }
        public String City { get; set; }
        public String Mascot { get; set; }
        public int DraftKingsIdentifier { get; set; }
        public String DraftKingsAbbr { get; set; }
        public String EspnAbbr { get; set; }
    }
}
