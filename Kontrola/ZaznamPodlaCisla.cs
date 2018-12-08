using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using audsSem2;

namespace Kontrola
{
    public class ZaznamPodlaCisla : IRecord<ZaznamPodlaCisla>
    {
        public ZaznamPodlaCisla(int adresa, int cislo)
        {
            Adresa = adresa;
            Cislo = cislo;
        }
        public ZaznamPodlaCisla(byte[] pole)
        {
            this.Cislo = BitConverter.ToInt32(pole, 0);
            this.Adresa = BitConverter.ToInt32(pole, 4);
        }
        public ZaznamPodlaCisla()
        {
            this.Cislo = -1;
            this.Adresa = -1;
        }
        public int  Adresa { get; set; }
        public int Cislo { get; set; }

        public byte[] GetHash()
        {
            return BitConverter.GetBytes(Cislo);
        }

        public bool Equals(ZaznamPodlaCisla rec)
        {
            return rec.Cislo == Cislo;
        }

        public byte[] ToByArray()
        {
            byte[] cis = BitConverter.GetBytes(Cislo);
            byte[] tx = BitConverter.GetBytes(Adresa);
            byte[] c = new byte[cis.Length + tx.Length];
            System.Buffer.BlockCopy(cis, 0, c, 0, cis.Length);
            System.Buffer.BlockCopy(tx, 0, c, cis.Length, tx.Length);
            return c;
        }

        public ZaznamPodlaCisla FromByArray(byte[] pole)
        {
            return new ZaznamPodlaCisla(pole);
        }

        public int GetSize()
        {
            return 8;
        }

        public ZaznamPodlaCisla DuplicujSA(byte[] data)
        {
            return new ZaznamPodlaCisla(data);
        }
        public override string ToString()
        {
            return "Identifikacne cislo: " + Cislo + "  Adresa: " + Adresa;
        }
    }
}
