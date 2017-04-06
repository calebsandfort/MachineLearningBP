using System.ComponentModel.DataAnnotations.Schema;

namespace MachineLearningBP.Entities.Sports.Nba
{
    [Table("NbaSeasons")]
    public class NbaSeason : Season<NbaStatLine, NbaSeason>
    {
    }
}
