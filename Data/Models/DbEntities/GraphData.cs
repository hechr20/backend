using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DbEntities
{
    public class GraphData : BaseEntity
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
}
