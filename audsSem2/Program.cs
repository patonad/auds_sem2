using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace audsSem2
{
    class Program
    {
        static void Main(string[] args)
        {
            //var hash = new DymHas(2);
            //while (true)
            //{
            //    //for (int i = 1; i < 11; i++)
            //    //{
            //    //    var c = new Record(i);
            //    //    hash.Add(c.GetHash());
            //    //}
            //    Console.WriteLine("Zadaj cislo");
            //    int a = Int32.Parse(Console.ReadLine());
            //    var b = new Record(a,"");
            //    hash.Add(b.GetHash());
            //}
            Record r = new Record(10,"aaaa");
            Record r1 = new Record(10, "bbbb");
            Record r2 = new Record(10,"cccc");
            Record r3 = new Record(10, "dddd");
            Block<Record> b = new Block<Record>(2, new Record());
            Block<Record> b1 = new Block<Record>(2, new Record());
            b.AddRecord(r);
            b.AddRecord(r1);
            b1.AddRecord(r2);
            b1.AddRecord(r3);
            Zapisovac<Record> zap = new Zapisovac<Record>("pukus.bin", new Block<Record>(2,new Record()));
            zap.OtvorZapis();
            zap.zapis(0,b.ToByteArrays());
            zap.zapis(1000, b1.ToByteArrays());
            zap.ZatvorZapis();
            zap.OtvorCitanie();
            Block<Record> novyBlok = new Block<Record>(zap.citaj(0),new Record());
            Block<Record> novyBlok1 = new Block<Record>(zap.citaj(1000), new Record());
            zap.ZatvorCitanie();
            

        }
    }
}
