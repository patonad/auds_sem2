using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    public class Block<T> where T : IRecord<T>
    {
        public T[] Records { get; set; }
        public int PocetRec { get; set; }
        public int PocetPlatnychRec { get; set; }
        public int SvojaAdresa { get; set; }
        public T Typ { get; set; }
        public Block( int pocetRec,int adresa, T data)
        {
            SvojaAdresa = adresa;
            Typ = data;
            PocetRec = pocetRec;
            Records = new T[pocetRec];
            for (int i = 0; i < pocetRec; i++)
            {
                Records[i] = data.DuplicujSA(data.ToByArray()) ;
            }
            PocetPlatnychRec = 0;
        }
        public Block(byte[] pole,T data,int adresa)
        {
            Typ = data;
            this.PocetRec = BitConverter.ToInt32(pole, 0);
            this.PocetPlatnychRec = BitConverter.ToInt32(pole, 4);
            SvojaAdresa = adresa;
            Records = new T[PocetRec];
            int a = 8;
            for (int i = 0; i < PocetRec; i++)
            {
                var b = pole.Skip(a).ToArray();
                b = b.Take(Typ.GetSize()).ToArray();
                Records[i] = data.DuplicujSA(b);
                a += data.GetSize();
            }

        }

        public void Swap(int index)
        {
            T pom = Records[index];
            Records[index] = Records[PocetPlatnychRec - 1];
            Records[PocetPlatnychRec - 1] = Typ;
            PocetPlatnychRec--;
        }

        public bool Vojde()
        {
            return PocetRec < PocetPlatnychRec;
        }

        public int GetSize()
        {
            int a = 0;
            foreach (var record in Records)
            {
                a += record.GetSize();
            }
            return a + 16 ;
        }

        public void AddRecord(T data)
        {
            Records[PocetPlatnychRec] = data;
            PocetPlatnychRec++;
        }

        public byte[] ToByteArrays()
        {
            
            byte[] c = new byte[GetSize()];
            byte[] prec = BitConverter.GetBytes(PocetRec);
            byte[] pprec = BitConverter.GetBytes(PocetPlatnychRec);
           
            int a = 0;
            System.Buffer.BlockCopy(prec, 0, c, a, 4);
            a += 4;
            System.Buffer.BlockCopy(pprec, 0, c, a, 4);
            a += 4;
            foreach (var record in Records)
            {
                System.Buffer.BlockCopy(record.ToByArray(), 0, c, a, record.GetSize());
                a += record.GetSize();
            }
            return c;
        }
    }
}
