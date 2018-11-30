using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    class ExternalNode : Node
    {
        public List<byte[]> pole;
        public ExternalNode(int hlbka)
        {
            base.HlbkaBloku = hlbka;
            PocetZaznamov = 0;
            pole =new List<byte[]>();
          
        }

        public int PocetZaznamov { get; set; }
        public int Adresa { get; set; }

        public void Pridaj(byte[]p)
        {
            pole.Add(p);

            PocetZaznamov++;
        }

    }
}
