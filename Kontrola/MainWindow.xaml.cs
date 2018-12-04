using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using audsSem2;

namespace Kontrola
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DymHas<Record> hash { get; set; }

        public void testuj()
        {
            for (int j = 0; j < 1; j++)
            {
                Random ran = new Random();
                List<int> stare = new List<int>();
                List<int> nove = new List<int>();
                for (int i = 1; i < 10000; i++)
                {
                    var a = new Record(ran.Next(10000, 11000), i.ToString("0000"));
                    if (!hash.Contiens(a))
                    {
                        hash.Add(a);
                        stare.Add(a.cislo);
                        var o = hash.Prever();
                        nove.Clear();
                        foreach (var block in o)
                        {
                            for (int k = 0; k < block.PocetPlatnychRec; k++)
                            {
                                nove.Add(block.Records[k].cislo);
                            }
                        }
                        nove.Sort();
                        stare.Sort();
                        for (int k = 0; k < nove.Count; k++)
                        {
                            if (nove[k] != stare[k])
                            {
                                return;
                            }
                        }
                    }
                }

                nove.Clear();

                for (int i = 0; i < stare.Count;)
                {
                    var index = ran.Next(0, stare.Count);
                    var a = stare[index];
                    stare.Remove(a);
                    hash.Delete(new Record(a, "0000"));
                    var o = hash.Prever();
                    nove.Clear();
                    foreach (var block in o)
                    {
                        for (int k = 0; k < block.PocetPlatnychRec; k++)
                        {
                            nove.Add(block.Records[k].cislo);
                        }
                    }
                    nove.Sort();
                    stare.Sort();
                    for (int k = 0; k < nove.Count; k++)
                    {
                        if (nove[k] != stare[k])
                        {
                            return;
                        }
                    }
                }

            }
            Prekresli();
        }
        public void testuj2()
        {
            Random ran = new Random(2);
            List<int> stare = new List<int>();
            List<int> nove = new List<int>();
            for (int i = 1; i < 15000; i++)
            {
                var a = new Record(ran.Next(10000, 15000), i.ToString("0000"));
                if (ran.Next(0, 11) <= 5)
                {
                    if (!hash.Contiens(a))
                    {
                        hash.Add(a);
                        stare.Add(a.cislo);
                        var o = hash.Prever();
                        nove.Clear();
                        foreach (var block in o)
                        {
                            for (int k = 0; k < block.PocetPlatnychRec; k++)
                            {
                                nove.Add(block.Records[k].cislo);
                            }
                        }

                        nove.Sort();
                        stare.Sort();
                        if (nove.Count != stare.Count)
                        {
                            return;
                        }

                        for (int k = 0; k < nove.Count; k++)
                        {
                            if (nove[k] != stare[k])
                            {
                                return;
                            }
                        }
                    }
                    nove.Clear();
                }
                else
                {
                    if (stare.Count != 0)
                    {
                        var index = ran.Next(0, stare.Count);
                        var b = stare[index];
                        stare.Remove(b);
                        hash.Delete(new Record(b, "0000"));
                        var o = hash.Prever();
                        nove.Clear();
                        foreach (var block in o)
                        {
                            for (int k = 0; k < block.PocetPlatnychRec; k++)
                            {
                                nove.Add(block.Records[k].cislo);
                            }
                        }

                        nove.Sort();
                        stare.Sort();
                        if (nove.Count != stare.Count)
                        {
                            return;
                        }
                        for (int k = 0; k < nove.Count; k++)
                        {
                            if (nove[k] != stare[k])
                            {
                                return;
                            }
                        }
                    }
                }
            }

            Prekresli();
        }
        public MainWindow()
        {

            InitializeComponent();
            hash = new DymHas<Record>(32, new Record(), 2, "pokus.bin", 3, "pokusPrepn.bin");
            //testuj2();
            //DymHas<Record> hash = new DymHas<Record>(20, new Record(), 2, "pokus.bin", 3, "pokusPrepn.bin");


            hash.Add(new Record(8, "4045"));
            hash.Add(new Record(16, "2047"));
            hash.Add(new Record(4, "cc015"));
            hash.Add(new Record(12, "dd04"));
            hash.Add(new Record(2, "ee05"));
            hash.Add(new Record(10, "ee06"));
            hash.Add(new Record(6, "ee07"));
            hash.Add(new Record(14, "ee08"));
            hash.Add(new Record(1, "ee05"));
            hash.Add(new Record(9, "ee06"));
            hash.Add(new Record(5, "ee07"));
            hash.Add(new Record(13, "ee08"));
            hash.Add(new Record(3, "ee09"));
            hash.Add(new Record(11, "ee10"));
            hash.Add(new Record(7, "ee10"));
            hash.Add(new Record(15, "ee10"));

            //hash.Delete(new Record(8, "0000"));
            //hash.Delete(new Record(4, "0000"));
            //hash.Delete(new Record(2, "0000"));
            //hash.Delete(new Record(6, "0000"));
            //hash.Delete(new Record(12, "0000"));
            //hash.Delete(new Record(10, "0000"));
            //hash.Delete(new Record(9, "0000"));
            //hash.Delete(new Record(5, "0000"));
            //hash.Delete(new Record(3, "0000"));
            //hash.Delete(new Record(7, "0000"));
            //hash.Delete(new Record(11, "0000"));
            //hash.Delete(new Record(1, "0000"));
            //hash.Delete(new Record(8, 8.ToString("0000")));
            //var list = hash.Prever();
            //var listZobraz = new List<string>();
            //listZobraz.Add("Adresa | Platne zaznamy | Pocet zaznamov | Preplnovaci blok | Data");
            //foreach (var block in list)
            //{
            //    string str = String.Format("{0,-10} | {1,-24} | {2,-26}| {3,-25} |", block.SvojaAdresa,
            //        block.PocetPlatnychRec, block.PocetRec, block.PreplnovaciBlok);
            //    foreach (var blockRecord in block.Records)
            //    {
            //        str += "   " + blockRecord.ToString();
            //    }

            //    listZobraz.Add(str);

            //}

            //DG.ItemsSource = listZobraz;
            Prekresli();

        }

        private void Prekresli()
        {
            var list = hash.Prever();
            if (list.Count != 0)
            {
                if (list.Last().PocetPlatnychRec == 0)
                {
                    return;
                }
            }

            var listZobraz = new List<string>();
            listZobraz.Add("Adresa | Platne zaznamy | Pocet zaznamov | Preplnovaci blok | Data");
            foreach (var block in list)
            {
                string str = String.Format("{0,-10} | {1,-24} | {2,-26}| {3,-25} |", block.SvojaAdresa,
                    block.PocetPlatnychRec, block.PocetRec, block.PreplnovaciBlok);
                foreach (var blockRecord in block.Records)
                {
                    str += "   " + blockRecord.ToString();
                }

                listZobraz.Add(str);

            }

            DG.ItemsSource = listZobraz;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //for (int j = 0; j < 10; j++)
            //{
            //    List<int> list = new List<int>();
            //    Random ran = new Random(j);
            //    if (j == 5)
            //    {
            //        var a = 1;
            //    }

            //    for (int i = 0; i < 10; i++)
            //    {

            //        if (i == 2)
            //        {
            //            var a = 0;
            //        }

            //        if (ran.Next(0, 6) <= 2)
            //        {
            //            var a = ran.Next(1, 5);
            //            if (!list.Contains(a))
            //                list.Add(a);
            //            hash.Add(new Record(a, "AAAA"));
            //        }
            //        else
            //        {
            //            var a = ran.Next(1, 5);
            //            if (list.Contains(a))
            //                list.Remove(a);
            //            hash.Delete(new Record(a, "AAAA"));
            //        }
            //    }
            if (Koho.Text != "")
                hash.Add(new Record(Int32.Parse(Koho.Text), "AAAA"));
            Prekresli();


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Koho.Text != "")
                hash.Delete(new Record(Int32.Parse(Koho.Text), "AAAA"));
            Prekresli();
        }
    }
}
