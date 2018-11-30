using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    class Zapisovac<T>where T : IRecord<T>
    {
        public Zapisovac(string nazovSuboru,Block<T> data)
        {
            Typ = data;
            NazovSuboru = nazovSuboru;
        }

        public void OtvorCitanie()
        {
            _reader = new BinaryReader(File.OpenRead(NazovSuboru));
        }
        public void OtvorZapis()
        {
            _writer = new BinaryWriter(File.OpenWrite(NazovSuboru));
        }
        public void ZatvorCitanie()
        {
            _reader.Close();
        }
        public void ZatvorZapis()
        {
            _writer.Flush();
            _writer.Close();
        }
        public Block<T> Typ{ get; set; }
        public String NazovSuboru { get; set; }
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public void zapis(int CisloBloku, byte[] pole){
            _writer.Seek(CisloBloku * pole.Length, SeekOrigin.Begin);
            _writer.Write(pole);
        }
        public byte[] citaj(int CisloBloku)
        {
            
            byte[] pole = new Byte[Typ.GetSize()] ;
            _reader.BaseStream.Seek(CisloBloku * pole.Length, SeekOrigin.Begin);
            return pole = _reader.ReadBytes(Typ.GetSize());

        }

    }
}
