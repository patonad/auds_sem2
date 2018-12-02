using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    class InternalNode : Node
    {
        public InternalNode(int hlbka)
        {
            base.HlbkaBloku = hlbka;
        }

        public Node Left { get; set; }
        public Node Right { get; set; }
    }
}
