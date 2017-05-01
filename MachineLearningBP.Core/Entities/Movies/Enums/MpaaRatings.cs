using MachineLearningBP.Framework;

namespace MachineLearningBP.Entities.Movies.Enums
{
   public enum MpaaRatings
   {
       [Display("None")]
       None,
       [Display("G")]
       G,
       [Display("PG-13")]
       PG_13,
       [Display("PG")]
       PG,
       [Display("R")]
       R,
       [Display("NC-17")]
       NC_17,
       [Display("Unrated")]
       Unrated,
   }
}
