using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    class Zapisovac<T> where T : IRecord<T>
    {
        public Zapisovac(string nazovSuboru, Block<T> data)
        {
            Typ = data;
            NazovSuboru = nazovSuboru;
            fs = new FileStream(nazovSuboru, FileMode.Create, FileAccess.ReadWrite,
                FileShare.Read);
            _reader = new BinaryReader(fs);
            _writer = new BinaryWriter(fs);
        }
        public void ZatvorZapis()
        {
            _writer.Flush();
            _writer.Close();
        }
        public Block<T> Typ { get; set; }
        public String NazovSuboru { get; set; }
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private FileStream fs;
        public void skrat(int a)
        {

            fs.SetLength(Math.Max(0, fs.Length - (Typ.GetSize()*a)));
            fs.Flush();

        }

        public void zapis(int CisloBloku, byte[] pole)
        {
            _writer.Seek(CisloBloku * pole.Length, SeekOrigin.Begin);
            _writer.Write(pole);
            _writer.Flush();
        }

        public byte[] celySubor()
        {
            _reader.Close();
            _writer.Close();

            var pole = File.ReadAllBytes(NazovSuboru);
            fs = new FileStream(NazovSuboru, FileMode.OpenOrCreate, FileAccess.ReadWrite,
               FileShare.Read);

            _reader = new BinaryReader(fs);
            _writer = new BinaryWriter(fs);
            return pole;
        }

        public byte[] citaj(int CisloBloku)
        {

            byte[] pole = new Byte[Typ.GetSize()];
            _reader.BaseStream.Seek(CisloBloku * pole.Length, SeekOrigin.Begin);
            return pole = _reader.ReadBytes(Typ.GetSize());


        }

    }
}