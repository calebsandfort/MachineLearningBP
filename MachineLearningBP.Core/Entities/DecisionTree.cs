using Abp.Domain.Entities;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using MachineLearningBP.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities
{
    [Table("DecisionTrees")]
    public class DecisionTree : Entity<int>
    {
        public DecisionTreeTargets Target { get; set; }

        public String RootJson { get; set; }
        [NotMapped]
        public DecisionNode Root
        {
            get
            {
                return JsonConvert.DeserializeObject<DecisionNode>(this.RootJson);
            }
            set
            {
                this.RootJson = JsonConvert.SerializeObject(value);
            }
        }
    }
}
