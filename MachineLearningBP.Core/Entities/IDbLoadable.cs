using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Entities
{
    public interface IDbLoadable
    {
        int LastId { get; }
    }
}
