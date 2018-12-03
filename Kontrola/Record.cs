using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    public class Record:IRecord<Record>
    {
        public Record(int cislo, string text)
        {
            this.cislo = cislo;
            this.text = text;
        }
        public Record()
        {
            this.cislo = -1;
            this.text = "Kopi";
        }
        public Record(byte[] pole)
        {
            this.cislo = BitConverter.ToInt32(pole,0);
            this.text = Encoding.ASCII.GetString(pole,4,4);
        }
        public int  cislo { get; set; }
        public string text { get; set; }
        public byte[] GetHash()
        {
            return BitConverter.GetBytes(cislo);
        }

        public bool Equals(Record rec)
        {
            return cislo == rec.cislo ? true : false;
        }

        public byte[] ToByArray()
        {
            byte[] cis = BitConverter.GetBytes(cislo);
            byte[] tx = Encoding.ASCII.GetBytes(text);
            byte[] c = new byte[cis.Length + tx.Length];
            System.Buffer.BlockCopy(cis, 0, c, 0, cis.Length);
            System.Buffer.BlockCopy(tx, 0, c, cis.Length, tx.Length);
            return c;
        }

        public Record FromByArray(byte[] pole)
        {
            return  new Record(pole);
        }

        public int GetSize()
        {
            return 8;
        }

        public Record DuplicujSA(byte[] data)
        {
            return new Record(data);
        }

        public override string ToString()
        {
            return String.Format("{0,-10}", cislo) + text;
        }
    }
}
