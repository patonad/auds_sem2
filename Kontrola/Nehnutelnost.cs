using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using audsSem2;

namespace Kontrola
{
    public class Nehnutelnost

    {
        public Nehnutelnost(byte[] pole)
        {
            this.SupCislo = BitConverter.ToInt32(pole, 0);
            this.IdenCislo = BitConverter.ToInt32(pole, 4);
            this.NazovKaT = Encoding.ASCII.GetString(pole, 8, 15);
            this.Popis = Encoding.ASCII.GetString(pole, 23, 20);
        }
        public Nehnutelnost(int supCislo, int idenCislo, string nazovKaT, string popis)
        {
            SupCislo = supCislo;
            NazovKaT = nazovKaT;
            if (NazovKaT.Length > 15)
            {
                throw new Exception("Dlhy nazov katastra");
            }

            while (NazovKaT.Length < 15)
            {
                NazovKaT += ";";
            }

            Popis = popis;
            if (popis.Length > 20)
            {
                throw new Exception("Dlhy popis");
            }

            while (Popis.Length < 20)
            {
                Popis += ";";
            }

            IdenCislo = idenCislo;
        }

        public int SupCislo { get; set; }
        public string NazovKaT { get; set; }
        public string Popis { get; set; }
        public int IdenCislo { get; set; }
        public override string ToString()
        {
            var n = NazovKaT.Replace(';',' ');
            var p = Popis.Replace(';', ' ');

            return "Identifikacne cislo: "+IdenCislo + "   Supistne cislo: " + SupCislo + "   Nazov katastru: " + n + "   Popis: " + p;
        }

        public bool Equals(Nehnutelnost rec)
        {
            return rec.IdenCislo == IdenCislo;
        }

        public byte[] ToByArray()
        {
            byte[] sup = BitConverter.GetBytes(SupCislo);
            byte[] idens = BitConverter.GetBytes(IdenCislo);
            byte[] nk = Encoding.ASCII.GetBytes(NazovKaT);
            byte[] pop = Encoding.ASCII.GetBytes(Popis);
            byte[] c = new byte[sup.Length + idens.Length +nk.Length+pop.Length];
            System.Buffer.BlockCopy(sup, 0, c, 0, sup.Length);
            System.Buffer.BlockCopy(idens, 0, c, 4, idens.Length);
            System.Buffer.BlockCopy(nk, 0, c, 8, nk.Length);
            System.Buffer.BlockCopy(pop, 0, c, 23, pop.Length);
            return c;
        }
        public int GetSize()
        {
            return 43;
        }


    }
}
