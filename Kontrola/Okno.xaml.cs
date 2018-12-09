using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using Visibility = System.Windows.Visibility;

namespace Kontrola
{
    /// <summary>
    /// Interaction logic for Okno.xaml
    /// </summary>
    public partial class Okno : Window
    {
        public Databazka DB { get; set; }
        public Okno()
        {
            InitializeComponent();
            //DB.PridajNehnutelnost(1, "Kataster1", "Popis1", 1);
            //DB.PridajNehnutelnost(2, "Kataster1", "Popis2", 2);
            //DB.PridajNehnutelnost(3, "Kataster1", "Popis3", 3);
            //DB.PridajNehnutelnost(4, "Kataster1", "Popis4", 4);
            //DB.PridajNehnutelnost(5, "Kataster1", "Popis5", 5);
            //DB.PridajNehnutelnost(6, "Kataster1", "Popis6", 6);
            //DB.PridajNehnutelnost(1, "Kataster2", "Popis7", 7);
            //DB.PridajNehnutelnost(2, "Kataster2", "Popis8", 8);
            //DB.PridajNehnutelnost(3, "Kataster2", "Popis9", 9);
            //DB.PridajNehnutelnost(4, "Kataster2", "Popis10", 10);
            //DB.PridajNehnutelnost(5, "Kataster2", "Popis11", 11);
        }

        public int SC { get; set; }
        public int IC { get; set; }
        public string nazov { get; set; }
        public string  popis { get; set; }
        public void Schovaj()
        {
            Pridaj.Visibility = Visibility.Collapsed;
            Vyhladaj.Visibility = Visibility.Collapsed;
            Odstran.Visibility = Visibility.Collapsed;
            Vypis.Visibility = Visibility.Collapsed;
            Generuj.Visibility = Visibility.Collapsed;
        }
        // pridaj
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PIC.Text == "" || PSC.Text == "" || PPO.Text == "" || PNK.Text == "")
            {
                MessageBox.Show("Nezadal si daky udaj");
                return;
            }

            int sup;
            int iden;
            try
            {
                sup = Int32.Parse(PSC.Text);
                iden = Int32.Parse(PIC.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zle zadane cislo");
                return;
            }
            DB.PridajNehnutelnost(sup, PNK.Text, PPO.Text, iden);
            LWP.ItemsSource = DB.celySuborHlavnyPlatneZaznamy();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Schovaj();
            Pridaj.Visibility = Visibility.Visible;
            LWP.ItemsSource = DB.celySuborHlavnyPlatneZaznamy();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Schovaj();
            Vyhladaj.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Schovaj();
            Odstran.Visibility = Visibility.Visible;
            LWO.ItemsSource = DB.celySuborHlavnyPlatneZaznamy();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Schovaj();
            Vypis.Visibility = Visibility.Visible;
        }
        //vypis hlavny subor
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            LWVypis.ItemsSource = DB.celySuborHlavny();
        }
        //odstarn
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (ONK.Text == "" || OSC.Text == "")
            {
                MessageBox.Show("Nezadal si daky udaj");
                return;
            }
            int sup;
            try
            {
                sup = Int32.Parse(OSC.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zle zadane cislo");
                return;
            }
            DB.Odstran(sup,ONK.Text);
            LWO.ItemsSource = DB.celySuborHlavnyPlatneZaznamy();
        }
        //vyhladaj poda ic
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (VIC.Text == "")
            {
                MessageBox.Show("Nezadal si daky udaj");
                return;
            }
            int ic;
            try
            {
                ic = Int32.Parse(VIC.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zle zadane cislo");
                return;
            }

            var list = DB.Vyhladaj(ic);
            if (list != null)
            {
                VVIC.Text = list[0];
                VVSC.Text = list[1];
                VVNK.Text = list[2].Replace(";","");
                VVPO.Text = list[3].Replace(";","");
                IC = Int32.Parse(list[0]);
                SC = Int32.Parse(list[1]);
                nazov = list[2].Replace(";", "");
                popis = list[3].Replace(";", "");
            }

        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (VSC.Text == "" || VNK.Text == "")
            {
                MessageBox.Show("Nezadal si daky udaj");
                return;
            }
            int sc;
            try
            {
                sc = Int32.Parse(VSC.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zle zadane cislo");
                return;
            }

            var list = DB.Vyhladaj(sc,VNK.Text);
            if (list != null)
            {
                VVIC.Text = list[0];
                VVSC.Text = list[1];
                VVNK.Text = list[2].Replace(";", "");
                VVPO.Text = list[3].Replace(";", "");
                IC = Int32.Parse(list[0]);
                SC = Int32.Parse(list[1]);
                nazov = list[2].Replace(";", "");
                popis = list[3].Replace(";", "");
            }
        }
        //zmen
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            //nic
            if (VVIC.Text == IC.ToString() && VVSC.Text == SC.ToString() && VVNK.Text == nazov && VVPO.Text == popis)
            {
                return;
            }
            // iba popis
            if (VVIC.Text == IC.ToString() && VVSC.Text == SC.ToString() && VVNK.Text == nazov && VVPO.Text != popis)
            {
                DB.ZmenPopis(IC, VVPO.Text);
                return;
            }
            //vsetko
            if (VVIC.Text != IC.ToString() && (VVSC.Text != SC.ToString() || VVNK.Text != nazov))
            {
                int sc,ic;
                try
                {
                    sc = Int32.Parse(VVSC.Text);
                    ic = Int32.Parse(VVIC.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Zle zadane cislo");
                    return;
                }
                DB.ZmenVS(IC,ic,sc,VVNK.Text,VVPO.Text);
                return;
            }
            // nazov alebo sup
            if (VVSC.Text != SC.ToString() || VVNK.Text != nazov)
            {
                int sc;
                try
                {
                    sc = Int32.Parse(VVSC.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Zle zadane cislo");
                    return;
                }
                DB.ZmenNa(IC, sc, VVNK.Text, VVPO.Text);
                return;
            }
            if (VVIC.Text != IC.ToString())
            {
                int ic;
                try
                {
                    ic = Int32.Parse(VVIC.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Zle zadane cislo");
                    return;
                }
                DB.ZmenIC(IC, ic, VVPO.Text);
                return;
            }
        }
        // vypsi s nazvom
        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            LWVypis.ItemsSource = DB.VypisSuborSNazvom();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            LWVypis.ItemsSource = DB.VypisSuborIdent();
        }
        //generuj
        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            Schovaj();
            Generuj.Visibility = Visibility.Visible;
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (GPK.Text == "" || GPN.Text == "")
            {
                MessageBox.Show("Nezadal si daky udaj");
                return;
            }

            int neh;
            int kat;
            try
            {
                kat = Int32.Parse(GPK.Text);
                neh = Int32.Parse(GPN.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Zle zadane cislo");
                return;
                
            }

            if(DB !=null)
                DB.Zatvor();
            FileStream fs = new FileStream("data.bin", FileMode.Create);
            fs.Close();
            fs = new FileStream("ZaznamPodlaCisla.bin", FileMode.Create);
            fs.Close();
            fs = new FileStream("ZoznamPodlaNazvuACisla.bin", FileMode.Create);
            fs.Close();
            DB = new Databazka();
            DB.Generuj(kat,neh);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if(DB != null)
            DB.Uloz();
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            if (DB == null)
            {
                FileStream fs = new FileStream("hlavnySubor.csv", FileMode.Open);
                StreamReader hsr = new StreamReader(fs);
                DB = new Databazka(hsr);
                hsr.Close();
                fs.Close();
            }
        }
    }
}
