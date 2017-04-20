using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using MachineLearningBP.Entities.Sports.Nba;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Core.Entities.Sports.Nba.Dtos
{
    [AutoMapFrom(typeof(NbaGame))]
    public class NbaGameDto : EntityDto
    {
        [DisplayName("Away")]
        public String AwayTeam { get; set; }

        [DisplayName("Home")]
        public String HomeTeam { get; set; }

        [DisplayName("A Knn Pts")]
        public Double AwayKnnPoints { get; set; }

        [DisplayName("H Knn Pts")]
        public Double HomeKnnPoints { get; set; }

        [DisplayName("A Tree Pts")]
        public Double AwayTreePoints { get; set; }

        [DisplayName("H Tree Pts")]
        public Double HomeTreePoints { get; set; }

        public Double Spread { get; set; }
        public Double Total { get; set; }

        public Double KnnSpread { get { return this.AwayKnnPoints - this.HomeKnnPoints; } }
        public Double KnnTotal { get { return this.AwayKnnPoints + this.HomeKnnPoints; } }

        public Double TreeSpread { get { return this.AwayTreePoints - this.HomeTreePoints; } }
        public Double TreeTotal { get { return this.AwayTreePoints + this.HomeTreePoints; } }
    }
}
