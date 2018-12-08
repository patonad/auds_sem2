using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using audsSem2;

namespace Kontrola
{
    public class ZoznamPodlaNazvuACisla: IRecord<ZoznamPodlaNazvuACisla>
    {
        public ZoznamPodlaNazvuACisla(int adresa, int supCislo, string nazov)
        {
            Adresa = adresa;
            SupCislo = supCislo;
            Nazov = nazov;
        }
        public ZoznamPodlaNazvuACisla()
        {
            Adresa = -1;
            SupCislo = -1;
            Nazov = "eeeeeeeeeeeeeee";
        }
        public ZoznamPodlaNazvuACisla(byte[] pole)
        {
            this.Adresa = BitConverter.ToInt32(pole, 0);
            this.SupCislo = BitConverter.ToInt32(pole, 4);
            this.Nazov = Encoding.ASCII.GetString(pole, 8, 15);
        }
        public int Adresa { get; set; }
        public int SupCislo { get; set; }
        public string Nazov { get; set; }
        public byte[] GetHash()
        {
            byte[] s = BitConverter.GetBytes(SupCislo);
            byte[] n = Encoding.ASCII.GetBytes(Nazov);
            byte[] c = new byte[s.Length + n.Length];
            System.Buffer.BlockCopy(s, 0, c, 0, s.Length);
            System.Buffer.BlockCopy(n, 0, c, 4, n.Length);
            return c;
        }

        public bool Equals(ZoznamPodlaNazvuACisla rec)
        {
            return rec.Nazov == Nazov && SupCislo == rec.SupCislo;
        }

        public byte[] ToByArray()
        {
            byte[] adr = BitConverter.GetBytes(Adresa);
            byte[] s = BitConverter.GetBytes(SupCislo);
            byte[] n = Encoding.ASCII.GetBytes(Nazov);
            byte[] c = new byte[adr.Length + s.Length +n.Length];
            System.Buffer.BlockCopy(adr, 0, c, 0, adr.Length);
            System.Buffer.BlockCopy(s, 0, c, 4, s.Length);
            System.Buffer.BlockCopy(n, 0, c, 8, n.Length);
            return c;
        }

        public ZoznamPodlaNazvuACisla FromByArray(byte[] pole)
        {
            return new ZoznamPodlaNazvuACisla(pole);
        }

        public int GetSize()
        {
            return 23;
        }

        public ZoznamPodlaNazvuACisla DuplicujSA(byte[] data)
        {
            return new ZoznamPodlaNazvuACisla(data);
        }
        public override string ToString()
        {
            return "Adresa: " + Adresa + "  Supistne cislo: " + SupCislo + "   Nazov katastru: " + Nazov.Replace(';',' ');
            
        }
    }
}
