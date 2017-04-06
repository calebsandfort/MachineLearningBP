using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbTeams")]
    public class MlbTeam : Team<MlbStatLine, MlbSeason>
    {
    }
}
