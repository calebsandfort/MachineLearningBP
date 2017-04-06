using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaTeams")]
    public class NbaTeam : Team<NbaStatLine, NbaSeason>
    {
    }
}
