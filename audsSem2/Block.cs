using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    class Block<T> where T : IRecord<T>
    {
        public T[] Records { get; set; }
        public int PocetRec { get; set; }
        public int PocetPlatnychRec { get; set; }
        public int  PreplnovaciBlok { get; set; }
        public int SvojaAdresa { get; set; }
        public T Typ { get; set; }
        public Block( int pocetRec,int adresa, T data)
        {
            SvojaAdresa = adresa;
            PreplnovaciBlok = -1;
            Typ = data;
            PocetRec = pocetRec;
            Records = new T[pocetRec];
            for (int i = 0; i < pocetRec; i++)
            {
                Records[i] = data.DuplicujSA(data.ToByArray()) ;
            }
            PocetPlatnychRec = 0;
        }
        public Block(byte[] pole,T data)
        {
            PreplnovaciBlok = -1;
            Typ = data;
            this.PocetRec = BitConverter.ToInt32(pole, 0);
            this.PocetPlatnychRec = BitConverter.ToInt32(pole, 4);
            this.PreplnovaciBlok = BitConverter.ToInt32(pole, 8);
            this.SvojaAdresa = BitConverter.ToInt32(pole, 12);
            Records = new T[PocetRec];
            int a = 16;
            for (int i = 0; i < PocetRec; i++)
            {
                var b = pole.Skip(a).ToArray();
                b = b.Take(Typ.GetSize()).ToArray();
                Records[i] = data.DuplicujSA(b);
                a += data.GetSize();
            }

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
            byte[] prepBlok = BitConverter.GetBytes(PreplnovaciBlok);
            byte[] adresa = BitConverter.GetBytes(SvojaAdresa);
            int a = 0;
            System.Buffer.BlockCopy(prec, 0, c, a, 4);
            a += 4;
            System.Buffer.BlockCopy(pprec, 0, c, a, 4);
            a += 4;
            System.Buffer.BlockCopy(prepBlok, 0, c, a, 4);
            a += 4;
            System.Buffer.BlockCopy(adresa, 0, c, a, 4);
            a += 4;
            foreach (var record in Records)
            {
                System.Buffer.BlockCopy(record.ToByArray(), 0, c, a, record.GetSize());
                a += record.GetSize();
            }
            return c;
        }
        public Block<T> FromByArray(byte[] pole)
        {
            return new Block<T>(pole,Typ);
        }
    }
}
