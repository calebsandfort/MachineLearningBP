using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos
{
    public class Bucket
    {
        public Double Lower { get; set; }
        public Double Upper { get; set; }

        public bool Within(Double val)
        {
            return val >= this.Lower && val < this.Upper;
        }

        public override string ToString()
        {
            return $"{this.Lower:N1} => {this.Upper:N1}";
        }

        public static Bucket GetBucket(Double val, Double width = 3, int count = 200)
        {
            Bucket bucket = new Bucket();
            int halfCount = count / 2;

            if (val < 0)
            {
                for (int i = 0; i < halfCount - 1; i++)
                {
                    bucket = new Bucket { Lower = (-i - 1) * width, Upper = -i * width };
                    if (bucket.Within(val)) return bucket;
                } 
            }

            for (int i = 0; i < halfCount - 1; i++)
            {
                bucket = new Bucket { Lower = i * width, Upper = (i + 1) * width };
                if (bucket.Within(val)) return bucket;
            }

            return bucket;
        }
    }
}
