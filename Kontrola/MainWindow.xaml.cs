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
        public MainWindow()
        {
            InitializeComponent();

            DymHas<Record> hash = new DymHas<Record>(0, new Record(), 2, "pokus.bin", 3, "pokusPrepn.bin");
            //hash.Add(new Record(4095, "4045"));
            //hash.Add(new Record(2047, "2047"));
            //hash.Add(new Record(8191, "cc015"));
            //hash.Add(new Record(4, "dd04"));
            //hash.Add(new Record(5, "ee05"));
            //hash.Add(new Record(6, "ee06"));
            //hash.Add(new Record(7, "ee07"));
            //hash.Add(new Record(8, "ee08"));
            //hash.Add(new Record(9, "ee09"));
            //hash.Add(new Record(10, "ee10"));
            for (int i = 1; i < 25; i++)
            {
               // hash.FInd(new Record(i, "ssss"));
                hash.Add(new Record(i, i.ToString("0000")));
                //hash.FInd(new Record(i, "ssss"));
                //hash.FInd(new Record(32, "ssss"));
            }

            hash.FInd(new Record(8, "ssss"));
            hash.FInd(new Record(1, "ssss"));
            hash.FInd(new Record(26, "ssss"));
            var list = hash.Prever();
            var listZobraz = new List<string>();
            listZobraz.Add("Adresa | Platne zaznamy | Pocet zaznamov | Preplnovaci blok | Data");
            foreach (var block in list)
            {
                string str = String.Format("{0,-10} | {1,-24} | {2,-26}| {3,-25} |", block.SvojaAdresa, block.PocetPlatnychRec, block.PocetRec, block.PreplnovaciBlok);
                foreach (var blockRecord in block.Records)
                {
                    str += "   " + blockRecord.ToString();
                }
                listZobraz.Add(str);
       
            }

            DG.ItemsSource = listZobraz;


        }
    }
}
