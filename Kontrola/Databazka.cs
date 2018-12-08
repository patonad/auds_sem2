using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using audsSem2;

namespace Kontrola
{
    public class Databazka
    {
        public DymHas<ZaznamPodlaCisla> DHPodlaCisla { get; set; }
        public DymHas<ZoznamPodlaNazvuACisla> DHPodlaNazvu { get; set; }
        public int Adresa { get; set; }
        public List<int> volneAdresy;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        private FileStream fs;
        private string NazovSuboru;
        public Databazka()
        {
            Adresa = 0;
            volneAdresy = new List<int>();
            DHPodlaCisla = new DymHas<ZaznamPodlaCisla>(33,new ZaznamPodlaCisla(),4,"ZaznamPodlaCisla.bin", "ZaznamPodlaCislaDH");
            DHPodlaNazvu = new DymHas<ZoznamPodlaNazvuACisla>(153, new ZoznamPodlaNazvuACisla(), 4, "ZoznamPodlaNazvuACisla.bin", "ZoznamPodlaNazvuACislaDH");
            NazovSuboru = "data.bin";
            fs = new FileStream(NazovSuboru, FileMode.Create, FileAccess.ReadWrite,
                FileShare.Read);
            _reader = new BinaryReader(fs);
            _writer = new BinaryWriter(fs);
        }
        public void PridajVolnuAdresu(int data)
        {

            if (volneAdresy.Count == 0)
            {
                volneAdresy.Add(data);
            }
            else
            {
                int minimum = 0;
                int maximum = volneAdresy.Count;
                int index;
                while (true)
                {
                    int akt = (maximum + minimum) / 2;
                    int adr = volneAdresy[akt];
                    if (adr < data)
                    {
                        if (akt < volneAdresy.Count - 1)
                        {
                            minimum = akt + 1;
                        }
                        else
                        {
                            index = volneAdresy.Count;
                            break;
                        }
                    }
                    else
                    {
                        if (akt > 0)
                        {
                            maximum = akt;
                        }
                        else
                        {
                            index = 0;
                            break;
                        }
                    }

                    if (maximum == minimum)
                    {
                        index = maximum;
                        break;
                    }
                }
                volneAdresy.Insert(index, data);

            }
        }
        public int DajAdresu()
        {
            if (volneAdresy.Count == 0)
            {
                Adresa++;

                return Adresa - 1;
            }
            else
            {
                int a = volneAdresy[0];
                volneAdresy.Remove(a);
                return a;
            }
        }
        public void PridajNehnutelnost(int supCislo, string nazovKaT, string popis, int idenCislo)
        {
            Nehnutelnost neh = new Nehnutelnost(supCislo,idenCislo,nazovKaT,popis);
            var adr = DajAdresu();
            DHPodlaNazvu.Add(new ZoznamPodlaNazvuACisla(adr, neh.SupCislo, neh.NazovKaT));
            DHPodlaCisla.Add(new ZaznamPodlaCisla(adr, neh.IdenCislo));
            Zapis(adr,neh.ToByArray());

        }
        public List<Tuple<int, string>> celySuborHlavnyPlatneZaznamy()
        {
            _reader.Close();
            _writer.Close();
            var pole = File.ReadAllBytes(NazovSuboru);
            fs = new FileStream(NazovSuboru, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.Read);
            _reader = new BinaryReader(fs);
            _writer = new BinaryWriter(fs);
            var listZobraz = new List<Tuple<int, string>>();
            for (int i = 0; i < pole.Length / 43; i++)
            {
                var pomPole = new byte[43];
                System.Buffer.BlockCopy(pole, i * 43, pomPole, 0, 43);
                var neh = new Nehnutelnost(pomPole);
                if(neh.IdenCislo != -1)
                listZobraz.Add(new Tuple<int, string>(neh.IdenCislo, neh.ToString()));

            }

            return listZobraz;

        }
        public void Zapis(int adr, byte[] pole)
        {
            _writer.Seek(adr*43, SeekOrigin.Begin);
            _writer.Write(pole);
            _writer.Flush();
        }
        public Nehnutelnost citaj(int adr)
        {
            byte[] pole = new Byte[43];
            _reader.BaseStream.Seek(adr * pole.Length, SeekOrigin.Begin);
            pole = _reader.ReadBytes(43);
            Nehnutelnost neh = new Nehnutelnost(pole);
            return neh;
        }
        public List<string> Vyhladaj(int ic)
        {
            var zaz = DHPodlaCisla.FInd(new ZaznamPodlaCisla(-1, ic));
            if (zaz != null)
            {
                var neh = citaj(zaz.Adresa);
                List<string> list = new List<string>();
                list.Add(neh.IdenCislo.ToString());
                list.Add(neh.SupCislo.ToString());
                list.Add(neh.NazovKaT);
                list.Add(neh.Popis);
                return list;
            }

            return null;
        }
        public List<string> Vyhladaj(int sc, string nazov)
        {
            while (nazov.Length < 15)
            {
                nazov += ";";
            }
            var zaz = DHPodlaNazvu.FInd(new ZoznamPodlaNazvuACisla(-1, sc,nazov));
            if (zaz != null)
            {
                var neh = citaj(zaz.Adresa);
                List<string> list = new List<string>();
                list.Add(neh.IdenCislo.ToString());
                list.Add(neh.SupCislo.ToString());
                list.Add(neh.NazovKaT);
                list.Add(neh.Popis);
                return list;
            }

            return null;
        }
        public void ZmenIC(int sic,int nic,string popis)
        {
            var zaz = DHPodlaCisla.FInd(new ZaznamPodlaCisla(-1, sic));
            if (zaz != null)
            {
                while (popis.Length < 20)
                {
                    popis += ";";
                }
                var neh = citaj(zaz.Adresa);
                DHPodlaCisla.Delete(new ZaznamPodlaCisla(-1, neh.IdenCislo));
                neh.IdenCislo = nic;
                neh.Popis = popis;
                Zapis(zaz.Adresa, neh.ToByArray());
                DHPodlaCisla.Add(new ZaznamPodlaCisla(zaz.Adresa, neh.IdenCislo));

            }
        }
        public void ZmenNa(int ic, int sc,string nazov, string popis)
        {
            var zaz = DHPodlaCisla.FInd(new ZaznamPodlaCisla(-1, ic));
            if (zaz != null)
            {
                while (popis.Length < 20)
                {
                    popis += ";";
                }
                while (nazov.Length < 15)
                {
                    nazov += ";";
                }
                var neh = citaj(zaz.Adresa);
                DHPodlaNazvu.Delete(new ZoznamPodlaNazvuACisla(-1, neh.SupCislo, neh.NazovKaT));
                neh.SupCislo = sc;
                neh.NazovKaT = nazov;
                neh.Popis = popis;
                Zapis(zaz.Adresa, neh.ToByArray());
                DHPodlaNazvu.Add(new ZoznamPodlaNazvuACisla(zaz.Adresa, neh.SupCislo, neh.NazovKaT));

            }
        }
        public void ZmenPopis(int ic, string popis)
        {
            var zaz = DHPodlaCisla.FInd(new ZaznamPodlaCisla(-1, ic));
            if (zaz != null)
            {
                while (popis.Length < 20)
                {
                    popis += ";";
                }
                var neh = citaj(zaz.Adresa);
                neh.Popis = popis;
                Zapis(zaz.Adresa,neh.ToByArray());
         
            }

           
        }
        public void ZmenVS(int sic,int nic, int sc,string nazov, string popis)
        {
            var zaz = DHPodlaCisla.FInd(new ZaznamPodlaCisla(-1, sic));
            if (zaz != null)
            {
                while (popis.Length < 20)
                {
                    popis += ";";
                }
                while (nazov.Length < 15)
                {
                    nazov += ";";
                }
                var neh = citaj(zaz.Adresa);
                DHPodlaNazvu.Delete(new ZoznamPodlaNazvuACisla(-1,neh.SupCislo,neh.NazovKaT));
                DHPodlaCisla.Delete(new ZaznamPodlaCisla(-1,neh.IdenCislo));
                neh.IdenCislo = nic;
                neh.SupCislo = sc;
                neh.NazovKaT = nazov;
                neh.Popis = popis;
                Zapis(zaz.Adresa,neh.ToByArray());
                DHPodlaNazvu.Add(new ZoznamPodlaNazvuACisla(zaz.Adresa, neh.SupCislo, neh.NazovKaT));
                DHPodlaCisla.Add(new ZaznamPodlaCisla(zaz.Adresa, neh.IdenCislo));

            }


        }
        public void Odstran(int sup, string nazov)
        {
            Nehnutelnost neh = new Nehnutelnost(sup,-1,nazov,"");
            var zaz = DHPodlaNazvu.FInd(new ZoznamPodlaNazvuACisla(-1, neh.SupCislo, neh.NazovKaT));
            if (zaz != null)
            {
                neh = citaj(zaz.Adresa);
                Zapis(zaz.Adresa, new Nehnutelnost(-1, -1, "", "").ToByArray());
                PridajVolnuAdresu(zaz.Adresa);
                DHPodlaNazvu.Delete(zaz);
                DHPodlaCisla.Delete(new ZaznamPodlaCisla(-1, neh.IdenCislo));
                skratPole(zaz.Adresa);
            }

        }
        public List<Tuple<int, string>> celySuborHlavny()
        {
            _reader.Close();
            _writer.Close();
            var pole = File.ReadAllBytes(NazovSuboru);
            fs = new FileStream(NazovSuboru, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.Read);
            _reader = new BinaryReader(fs);
            _writer = new BinaryWriter(fs);
            var listZobraz = new List<Tuple<int, string>>();
            for (int i = 0; i < pole.Length / 43; i++)
            {
                var pomPole = new byte[43];
                System.Buffer.BlockCopy(pole, i * 43, pomPole, 0, 43);
                var neh = new Nehnutelnost(pomPole);
                listZobraz.Add(new Tuple<int, string>(neh.IdenCislo,"Adresa: "+i+"  " +neh.ToString()));

            }

            return listZobraz;

        }
        public List<Tuple<int, string>> VypisSuborSNazvom()
        {
            var list = DHPodlaNazvu.Prever();
            var listZobraz = new List<Tuple<int, string>>();
            listZobraz.Add(new Tuple<int, string>(2, "Adresa | Platne zaznamy | Pocet zaznamov | Data"));
            foreach (var block in list)
            {
                string str = String.Format("{0,-10} | {1,-24} | {2,-26}| ", block.SvojaAdresa,
                    block.PocetPlatnychRec, block.PocetRec);
                foreach (var blockRecord in block.Records)
                {
                    str += "   " + blockRecord.ToString();
                }
                listZobraz.Add(new Tuple<int, string>(block.PocetPlatnychRec, str));

            }
            return listZobraz;

        }
        public List<Tuple<int, string>> VypisSuborIdent()
        {
            var list = DHPodlaCisla.Prever();
            var listZobraz = new List<Tuple<int, string>>();
            listZobraz.Add(new Tuple<int, string>(2, "Adresa | Platne zaznamy | Pocet zaznamov | Data"));
            foreach (var block in list)
            {
                string str = String.Format("{0,-10} | {1,-24} | {2,-26}| ", block.SvojaAdresa,
                    block.PocetPlatnychRec, block.PocetRec);
                foreach (var blockRecord in block.Records)
                {
                    str += "   " + blockRecord.ToString();
                }
                listZobraz.Add(new Tuple<int, string>(block.PocetPlatnychRec, str));

            }
            return listZobraz;

        }
        public void skrat(int a)
        {
            fs.SetLength(Math.Max(0, fs.Length - (
43 * a)));
            fs.Flush();

        }
        public bool skratPole(int adresa)
        {
            bool ret = false;
            if (volneAdresy.Count != 0)
            {
                int a = 0;
                while (volneAdresy.Last() == Adresa - 1)
                {
                    volneAdresy.Remove(volneAdresy.Last());
                    Adresa--;
                    a++;
                    if (volneAdresy.Count == 0)
                    {
                        break;
                    }
                }
                if (a != 0)
                {
                    skrat(a);
                    ret = true;
                }
            }
            return ret;
        }

        public void Generuj(int kat, int neh)
        {
            int a = 0;
            for (int i = 1; i < kat + 1; i++)
            {
                for (int j = 1; j < neh+1; j++)
                {
                    PridajNehnutelnost(j, "Kataster" + i,"Popis"+a,a);
                    a++;
                }
            }
        }
    }
}
