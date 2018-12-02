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
            //Record r = new Record(10, "aaaa");
            //Record r1 = new Record(10, "bbbb");
            //Record r2 = new Record(10, "cccc");
            //Record r3 = new Record(10, "dddd");
            //Block<Record> b = new Block<Record>(2, new Record());
            //Block<Record> b1 = new Block<Record>(2, new Record());
            //b.AddRecord(r);
            //b.AddRecord(r1);
            //b1.AddRecord(r2);
            //b1.AddRecord(r3);
            //Zapisovac<Record> zap = new Zapisovac<Record>("pukus.bin", new Block<Record>(2, new Record()));
            //zap.zapis(0, b.ToByteArrays());
            //zap.zapis(1000, b1.ToByteArrays());
            //Block<Record> novyBlok = new Block<Record>(zap.citaj(0), new Record());
            //Block<Record> novyBlok1 = new Block<Record>(zap.citaj(1000), new Record());
        


            DymHas<Record> hash = new DymHas<Record>(2,2,new Record(),"pokus.bin");
            hash.Add(new Record(4095, "4045"));
            hash.Add(new Record(2047, "2047"));
            hash.Add(new Record(8191, "cc015"));
            //hash.Add(new Record(4, "dd04"));
            //hash.Add(new Record(5, "ee05"));
            //hash.Add(new Record(6, "ee06"));
            //hash.Add(new Record(7, "ee07"));
            //hash.Add(new Record(8, "ee08"));
            //hash.Add(new Record(9, "ee09"));
            //hash.Add(new Record(10, "ee10"));

            //hash.Add(new Record(1, "aa01"));
            //hash.Add(new Record(2, "bb02"));
            //hash.Add(new Record(3, "cc03"));
            //hash.Add(new Record(4, "dd04"));
            //hash.Add(new Record(5, "ee05"));
            //hash.Add(new Record(6, "ee06"));
            //hash.Add(new Record(7, "ee07"));
            //hash.Add(new Record(8, "ee08"));
            //hash.Add(new Record(9, "ee09"));
            //hash.Add(new Record(10, "ee10"));
            //hash.Add(new Record(11, "ee11"));
            //hash.Add(new Record(12, "ee12"));
            //hash.Add(new Record(13, "ee13"));
            //hash.Add(new Record(14, "ee14"));
            //hash.Add(new Record(15, "ee15"));
            //hash.Add(new Record(16, "ee16"));

            //hash.add(new Record(4, 3"dddd"));
            //hash.add(new Record(5, "eeee"));

        }
    }
}
