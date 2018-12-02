using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    class ExternalNode : Node
    {
      
        public ExternalNode(int hlbka, int adresa)
        {
            Adresa = adresa;
            base.HlbkaBloku = hlbka;
            PocetZaznamov = 0;
        }

        public int PocetZaznamov { get; set; }
        public int Adresa { get; set; }
        public void Pridaj()
        {
            PocetZaznamov++;
        }

    }
}
