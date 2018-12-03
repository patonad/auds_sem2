using System;
using System.Collections.Generic;
using System.Linq;
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

        public MainWindow()
        {
            InitializeComponent();

            hash = new DymHas<Record>(20, new Record(), 2, "pokus.bin", 3, "pokusPrepn.bin");
            hash.Add(new Record(8, "4045"));
            hash.Add(new Record(16, "2047"));
            hash.Add(new Record(4, "cc015"));
            hash.Add(new Record(12, "dd04"));
            hash.Add(new Record(2, "ee05"));
            hash.Add(new Record(10, "ee06"));
            hash.Add(new Record(6, "ee07"));
            hash.Add(new Record(14, "ee08"));
            hash.Add(new Record(15, "ee09"));
            hash.Add(new Record(7, "ee10"));
            hash.Add(new Record(3, "ee10"));
            for (int i = 1; i < 0; i++)
            {
                hash.Add(new Record(i, i.ToString("0000")));
                hash.Contiens(new Record(i, i.ToString("0000")));
            }

            var list = hash.Prever();
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

        private void Prekresli()
        {
            var list = hash.Prever();
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
            List<int> list = new List<int>();
            Random ran =new Random(2);
            for (int i = 0; i < 100; i++)
            {
               
                    if (i == 2)
                    {
                        var a = 0;
                    }

                    if (ran.Next(0, 6) <= 2)
                    {
                        var a = ran.Next(0, 4);
                        if(!list.Contains(a))
                            list.Add(a);
                        hash.Add(new Record(a, "AAAA"));
                    }
                    else
                    {
                        var a = ran.Next(0, 4);
                        if (list.Contains(a))
                            list.Remove(a);
                        hash.Delete(new Record(a, "AAAA"));
                    }
                
               
                
            }

            if (Koho.Text != "")
            //    hash.Add(new Record(Int32.Parse(Koho.Text), "AAAA"));
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
