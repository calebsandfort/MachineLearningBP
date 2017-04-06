using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Mlb
{
    [Table("MlbSeasons")]
    public class MlbSeason : Season<MlbStatLine, MlbSeason>
    {
    }
}
