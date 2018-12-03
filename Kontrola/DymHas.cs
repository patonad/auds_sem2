using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;


namespace audsSem2
{
    public class DymHas<T> where T : IRecord<T>
    {

        private Zapisovac<T> _zapisovac;
        public int Adresa { get; set; }
        public Node Root { get; set; }
        public int PocetVBloku { get; set; }
        private T _typ;
        public List<int> volneBloky;
        public int DlzkaHashu { get; set; }
        public int PoslednaAdresa { get; set; }

        public DymHas(int hlbka, T data, int pocetVBloku, string adresaSuboru, int pocetVPrepnlovacomBloku, string adresaPreplnovaciehoSuboru)
        {
            DlzkaHashu = hlbka;
            volneBloky = new List<int>();
            _typ = data;
            _zapisovac = new Zapisovac<T>(adresaSuboru, new Block<T>(pocetVBloku, 0, data));
            PocetVBloku = pocetVBloku;
            Adresa = 0;
        }
        public int AkyBit(int pozicia, byte[] pole)
        {
            return (pole[pozicia / 8] & (1 << pozicia % 8)) != 0 ? 1 : 0;
        }


        //public bool add(T data)
        //{
        //    // ak je root prazdny
        //    if (Root == null)
        //    {
        //        Root = new ExternalNode(0, Adresa);
        //        ((ExternalNode) Root).PocetZaznamov++;
        //        Adresa++;
        //        Block<T> blok = new Block<T>(PocetVBlokuVBloku, _typ);
        //        blok.AddRecord(data);
        //        _zapisovac.zapis(0, blok.ToByteArrays());
        //        Console.WriteLine("Vlozil do roota prvy");
        //        return true;
        //    }
        //    // ak root nieje prazdny
        //    if (Root is ExternalNode)
        //    {
        //        // ak vojde
        //        if (((ExternalNode) Root).PocetZaznamov < PocetVBlokuVBloku)
        //        {
        //            Block<T> blok = new Block<T>(_zapisovac.citaj(((ExternalNode) Root).Adresa), _typ);
        //            blok.AddRecord(data);
        //            _zapisovac.zapis(0, blok.ToByteArrays());
        //            ((ExternalNode) Root).PocetZaznamov++;
        //            Console.WriteLine("Vlozil do roota dalsi");
        //            return true;
        //        }
        //        else
        //        {
        //            var node = NajdiExternalNode(data,Root);
        //            Block<T> Lblok = null;
        //            Block<T> Rblok = null;
        //            while (true)
        //            {
        //                Console.WriteLine("Rozsirujem sa");
        //                var inter = new InternalNode(node.HlbkaBloku);
        //                Block<T> Sblok = new Block<T>(_zapisovac.citaj(((ExternalNode)node).Adresa), _typ);
        //                foreach (var zaznam in Sblok.Records)
        //                {
        //                    if (AkyBit(node.HlbkaBloku, zaznam.GetHash()) == 0)
        //                    {
        //                        if (Lblok == null)
        //                        {
        //                            Lblok = new Block<T>(PocetVBlokuVBloku, _typ);
        //                            if (inter.Right == null)
        //                            {
        //                                inter.Left = new ExternalNode(node.HlbkaBloku + 1, ((ExternalNode)node).Adresa);
        //                            }
        //                            else
        //                            {
        //                                inter.Left = new ExternalNode(node.HlbkaBloku + 1, Adresa);
        //                                Adresa++;
        //                            }
        //                        }
        //                        Lblok.AddRecord(zaznam);
        //                        ((ExternalNode)inter.Left).PocetZaznamov++;
        //                        Console.WriteLine("Prehadzujem lavy");
        //                    }
        //                    else
        //                    {
        //                        if (Rblok == null)
        //                        {
        //                            Rblok = new Block<T>(PocetVBlokuVBloku, _typ);
        //                            if (inter.Left == null)
        //                            {
        //                                inter.Right = new ExternalNode(node.HlbkaBloku + 1, ((ExternalNode)node).Adresa);
        //                            }
        //                            else
        //                            {
        //                                inter.Right = new ExternalNode(node.HlbkaBloku + 1, Adresa);
        //                                Adresa++;
        //                            }

        //                        }

        //                        Rblok.AddRecord(zaznam);
        //                        ((ExternalNode)inter.Right).PocetZaznamov++;
        //                        Console.WriteLine("Prehadzujem pravy");
        //                    }
        //                }

        //                if (Root == node)
        //                {
        //                    Root = inter;
        //                }
        //                else
        //                {
        //                    if (((InternalNode)node).Left == node)
        //                    {
        //                        ((InternalNode)poNode).Left = inter;
        //                        node = inter;
        //                    }
        //                    else
        //                    {
        //                        ((InternalNode)poNode).Right = inter;
        //                        node = inter;
        //                    }

        //                }





        //                //if ((ExternalNode)inter.Left != null)
        //                //    _zapisovac.zapis(((ExternalNode)inter.Left).Adresa, Lblok.ToByteArrays());
        //                //if ((ExternalNode)inter.Right != null)
        //                //    _zapisovac.zapis(((ExternalNode)inter.Right).Adresa, Rblok.ToByteArrays());
        //            }
        //    }
        //    return true;
        //}
        public void PridajVolnuAdresu(int data)
        {
            volneBloky.Add(data);
        }
        public int DajAdresu()
        {
            if (volneBloky.Count == 0)
            {
                Adresa++;

                return Adresa - 1;
            }
            else
            {
                int a = volneBloky.Min();
                volneBloky.Remove(a);
                return a;
            }
        }

        public ExternalNode najdiNode(T data)
        {
            var node = Root;
            while (node is InternalNode)
            {
                if (AkyBit(node.HlbkaBloku, data.GetHash()) == 0)
                {
                    node = ((InternalNode)node).Left;
                }
                else
                {
                    node = ((InternalNode)node).Right;
                }

            }

            return (ExternalNode)node;
        }

        public bool skratPole(int adresa)
        {
            bool ret = false;
            //if (Adresa - 1 == adresa)
            //{
            //    _zapisovac.skrat();
            //    Adresa--;
            //    volneBloky.Remove(adresa);
            //    ret = true;

            //}

            if (volneBloky.Count != 0)
            {
                while (volneBloky.Max() == Adresa - 1)
                {
                    _zapisovac.skrat();
                    volneBloky.Remove(volneBloky.Max());
                    Adresa--;
                    ret = true;
                    if (volneBloky.Count == 0)
                    {
                        return ret;
                    }
                }
            }

            return ret;
        }

        public ExternalNode Rozsir(ref Block<T> blok, T data, ExternalNode node)
        {
            var pomNode = node;
            while (true)
            {
                Block<T> blok1 = new Block<T>(PocetVBloku, -1, _typ);
                Block<T> blok2 = new Block<T>(PocetVBloku, -1, _typ);
                foreach (var blokRecord in blok.Records)
                {
                    if (AkyBit(node.HlbkaBloku, blokRecord.GetHash()) == 0)
                    {
                        blok1.AddRecord(blokRecord);
                    }
                    else
                    {
                        blok2.AddRecord(blokRecord);
                    }
                }
                InternalNode roo = new InternalNode(node.HlbkaBloku);
                roo.Left = new ExternalNode(roo.HlbkaBloku + 1, -1);
                roo.Left.Parent = roo;
                roo.Right = new ExternalNode(roo.HlbkaBloku + 1,-1);
                roo.Right.Parent = roo;

                if (AkyBit(node.HlbkaBloku, data.GetHash()) == 0)//pasnw tu
                {
                    if (blok1.PocetPlatnychRec < PocetVBloku)
                    {
                        if (blok2.PocetPlatnychRec !=0)
                        {
                            if (((ExternalNode) roo.Left).Adresa == -1)
                            {
                                ((ExternalNode) roo.Right).Adresa = node.Adresa;
                                blok2.SvojaAdresa = ((ExternalNode) roo.Right).Adresa;
                                ((ExternalNode)roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
                                _zapisovac.zapis(blok2.SvojaAdresa, blok2.ToByteArrays());
                            }
                            else
                            {
                                ((ExternalNode)roo.Right).Adresa = DajAdresu();
                                blok2.SvojaAdresa = ((ExternalNode)roo.Right).Adresa;
                                ((ExternalNode)roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
                                _zapisovac.zapis(blok2.SvojaAdresa, blok2.ToByteArrays());
                            }
                        }
                        blok = blok1;
                        if (node == Root)
                        {
                            Root = roo;
                        }
                        else
                        {
                            if (((InternalNode)node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Right = roo;
                            }
                        }
                        if (((ExternalNode)roo.Right).Adresa == -1)
                        {
                            ((ExternalNode)roo.Left).Adresa = node.Adresa;
                            blok1.SvojaAdresa = ((ExternalNode)roo.Left).Adresa;
                        }
                        else
                        {
                            ((ExternalNode)roo.Left).Adresa = DajAdresu();
                            blok1.SvojaAdresa = ((ExternalNode)roo.Left).Adresa;
                        }
                            roo.Left.Parent = roo;
                            ((ExternalNode) roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
                            return (ExternalNode)roo.Left;  //tuto
                    }
                    else
                    {
                        blok = blok1;
                        if (node == Root)
                        {
                            Root = roo;
                        }
                        else
                        {
                            if (((InternalNode)node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Right = roo;
                            }
                        }

                        var a = node.Adresa;
                        node = (ExternalNode)roo.Left;
                        node.PocetZaznamov = blok.PocetPlatnychRec;
                        node.Adresa = a;
                    }
                }
                else
                {
                    if (blok2.PocetPlatnychRec < PocetVBloku)
                    {
                        if (blok1.PocetPlatnychRec != 0)
                        {
                            if (((ExternalNode)roo.Right).Adresa == -1)
                            {
                                ((ExternalNode)roo.Left).Adresa = node.Adresa;
                                blok1.SvojaAdresa = ((ExternalNode)roo.Left).Adresa;
                                ((ExternalNode) roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
                                _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
                            }
                            else
                            {
                                ((ExternalNode)roo.Left).Adresa = DajAdresu();
                                blok1.SvojaAdresa = ((ExternalNode)roo.Left).Adresa;
                                ((ExternalNode)roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
                                _zapisovac.zapis(blok2.SvojaAdresa, blok2.ToByteArrays());
                            }
                        }
                        blok = blok2;
                        if (node == Root)
                        {
                            Root = roo;
                        }
                        else
                        {
                            if (((InternalNode)node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Right = roo;
                            }
                        }
                        if (((ExternalNode)roo.Left).Adresa == -1)
                        {
                            ((ExternalNode)roo.Right).Adresa = node.Adresa;
                            blok2.SvojaAdresa = ((ExternalNode)roo.Right).Adresa;
                        }
                        else
                        {
                            ((ExternalNode)roo.Right).Adresa = DajAdresu();
                            blok2.SvojaAdresa = ((ExternalNode)roo.Right).Adresa;
                        }
                        roo.Right.Parent = roo;
                        ((ExternalNode) roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
                        return (ExternalNode)roo.Right;  //tuto
                    }
                    else
                    {
                        blok = blok2;
                        if (node == Root)
                        {
                            Root = roo;
                        }
                        else
                        {
                            if (((InternalNode)node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode)node.Parent).Right = roo;
                            }
                        }

                        var a = node.Adresa;
                        node = (ExternalNode)roo.Right;
                        node.PocetZaznamov = blok.PocetPlatnychRec;
                        node.Adresa = a;
                    }
                }

            }

        }

        public ExternalNode NajdiExternalNode(T data, ref Block<T> blok)
        {
            if (Root == null)
            {
                blok = new Block<T>(PocetVBloku, DajAdresu(), _typ);
                return new ExternalNode(0, blok.SvojaAdresa);
            }
            if (Root is ExternalNode)
            {
                if (((ExternalNode)Root).PocetZaznamov < PocetVBloku)
                {
                    blok = new Block<T>(_zapisovac.citaj(((ExternalNode)Root).Adresa), _typ);
                    return (ExternalNode)Root;
                }
                else
                {
                    blok = new Block<T>(_zapisovac.citaj(((ExternalNode)Root).Adresa), _typ);
                    if (Root.HlbkaBloku == DlzkaHashu)
                    {
                        return (ExternalNode)Root;
                    }
                    else
                    {
                        var a = Rozsir(ref blok, data, (ExternalNode) Root);
                        if (a.Adresa == -1)
                        {
                            blok = new Block<T>(PocetVBloku,DajAdresu(),_typ);
                            a.Adresa = blok.SvojaAdresa;
                        }
                        return a;
                    }
                }
            }
            else
            {
                var node = najdiNode(data);
                if (node.Adresa == -1)
                {
                    node.Adresa = DajAdresu();
                    blok = new Block<T>(PocetVBloku,node.Adresa, _typ);
                    return node;
                }
                if (node.PocetZaznamov < PocetVBloku)
                {
                    blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ);
                    return node;
                }
                else
                {
                    blok = new Block<T>(_zapisovac.citaj(((ExternalNode)node).Adresa), _typ);
                    if (node.HlbkaBloku == DlzkaHashu)
                    {
                        return (ExternalNode)node;
                    }
                    else
                    {
                        node = Rozsir(ref blok, data, node);
                        return node;
                    }
                }
            }
        }

        private ExternalNode FindDelete(T data, ref Block<T> blok)
        {
            if (Root != null)
            {
                var node = najdiNode(data);
                if (node.Adresa == -1)
                {
                    return null;
                }

                blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ);
                while (true)
                {
                    foreach (var blokRecord in blok.Records)
                    {
                        if (blokRecord.Equals(data))
                        {
                            return node;
                        }
                    }

                    if (blok.PreplnovaciBlok == -1)
                    {
                        return null;
                    }
                    else
                    {
                        blok = new Block<T>(_zapisovac.citaj(blok.PreplnovaciBlok), _typ);
                    }

                }
            }
            else
            {
                return null;
            }
        }

        public T FInd(T data)
        {
            if (Root != null)
            {
                var node = najdiNode(data);
                if (node.Adresa == -1)
                {
                    return default(T);
                }

                var blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ);
                while (true)
                {
                    foreach (var blokRecord in blok.Records)
                    {
                        if (blokRecord.Equals(data))
                        {
                            return blokRecord;
                        }
                    }

                    if (blok.PreplnovaciBlok == -1)
                    {
                        return default(T);
                    }
                    else
                    {
                        blok = new Block<T>(_zapisovac.citaj(blok.PreplnovaciBlok), _typ);
                    }

                }
            }
            else
            {
                return default(T);
            }
        }

        public bool Contiens(T data)
        {
            if (Root != null)
            {
                var node = najdiNode(data);
                if (node == null)
                    return false;
                if (node.Adresa == -1)
                {
                    return false;
                }

                var blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ);
                while (true)
                {
                    foreach (var blokRecord in blok.Records)
                    {
                        if (blokRecord.Equals(data))
                        {
                            return true;
                        }
                    }

                    if (blok.PreplnovaciBlok == -1)
                    {
                        return false;
                    }
                    else
                    {
                        blok = new Block<T>(_zapisovac.citaj(blok.PreplnovaciBlok), _typ);
                    }

                }
            }
            else
            {
                return false;
            }
        }

        public List<Block<T>> Prever()
        {
            var a = _zapisovac.celySubor();
            var typ = new Block<T>(PocetVBloku, -1, _typ);
            var cislo = typ.GetSize();
            var ret = new List<Block<T>>();
            for (int i = 0; i < a.Length / cislo; i++)
            {
                var pole = new byte[cislo];
                System.Buffer.BlockCopy(a, i * cislo, pole, 0, cislo);
                var blok = new Block<T>(pole, _typ);
                ret.Add(blok);
            }

            return ret;
        }

        public void zatvor()
        {
          
        }

        public void Delete(T data)
        {
            Block<T> blok = null;
            var node = FindDelete(data, ref blok);
            if (node != null)
            {
                int index = -1;
                for (int i = 0; i < blok.Records.Length; i++)
                {
                    if (blok.Records[i].Equals(data))
                    {
                        index = i;
                        break;
                    }
                }
                blok.Swap(index);
                node.PocetZaznamov--;
                if (node == Root)
                {
                    if (node.PocetZaznamov == 0)
                    {
                        PridajVolnuAdresu(node.Adresa);
                        skratPole(node.Adresa);
                        Root = null;
                        return;
                    }
                    else
                    {
                        _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                        return;
                    }
                }

                InternalNode par = (InternalNode)node.Parent;
                // ci som lavz alebo pravy
                while (true)
                {
                    if (par.Right == node)
                    {
                        if (par.Left is InternalNode)
                        {
                            _zapisovac.zapis(blok.SvojaAdresa,blok.ToByteArrays());
                            return;
                        }
                        if(((ExternalNode)par.Left).Adresa ==- 1)
                        {
                            if (par.Parent == null)
                            {
                                Root = node;
                                node.Parent = null;
                                node.HlbkaBloku--;
                                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                node.Adresa = blok.SvojaAdresa;
                                node.PocetZaznamov = blok.PocetPlatnychRec;
                                return;
                            }
                            if (((InternalNode)par.Parent).Left == par)
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode)par.Parent).Left = node;
                                par = (InternalNode)par.Parent;
                                node.Parent = par;
                            }
                            else
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode)par.Parent).Right = node;
                                par = (InternalNode)par.Parent;
                                node.Parent = par;
                            }
                        }
                        else
                        {
                            if ((par.Left as ExternalNode).PocetZaznamov + node.PocetZaznamov <= PocetVBloku)
                            {
                                Block<T> blok1 = new Block<T>(_zapisovac.citaj((par.Left as ExternalNode).Adresa), _typ);// nacital som bloky aj nevalidne
                                if (blok.SvojaAdresa < blok1.SvojaAdresa)
                                {
                                    for (int i = 0; i < blok1.PocetPlatnychRec; i++)
                                    {
                                        blok.AddRecord(blok1.Records[i]);
                                    }

                                    blok1.PocetPlatnychRec = 0;
                                    PridajVolnuAdresu(blok1.SvojaAdresa);
                                    if (!skratPole(blok1.SvojaAdresa))
                                    {
                                        _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < blok.PocetPlatnychRec; i++)
                                    {
                                        blok1.AddRecord(blok.Records[i]);
                                    }
                                    blok.PocetPlatnychRec = 0;
                                    PridajVolnuAdresu(blok.SvojaAdresa);
                                    if (!skratPole(blok.SvojaAdresa))
                                    {
                                        _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                    }
                                    blok = blok1;
                                }
                                if (par.Parent == null)
                                {
                                    Root = node;
                                    node.Parent = null;
                                    node.HlbkaBloku--;
                                    _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                    node.Adresa = blok.SvojaAdresa;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    return;
                                    
                                }
                                if (((InternalNode)par.Parent).Left == par)
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode)par.Parent).Left = node;
                                    par = (InternalNode)par.Parent;
                                    node.Parent = par;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                }
                                else
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode)par.Parent).Right = node;
                                    par = (InternalNode)par.Parent;
                                    node.Parent = par;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                }
                            }
                            else
                            {
                                _zapisovac.zapis(blok.SvojaAdresa,blok.ToByteArrays());
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (par.Right is InternalNode)
                        {
                            _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                            return;
                        }
                        if (((ExternalNode)par.Right).Adresa == -1)
                        {
                            if (par.Parent == null)
                            {
                                Root = node;
                                node.HlbkaBloku--;
                                node.Parent = null;
                                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                node.Adresa = blok.SvojaAdresa;
                                node.PocetZaznamov = blok.PocetPlatnychRec;
                                return;
                            }
                            if (((InternalNode)par.Parent).Left == par)
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode)par.Parent).Left = node;
                                par = (InternalNode)par.Parent;
                                node.Parent = par;
                                node.PocetZaznamov = blok.PocetPlatnychRec;
                                node.Adresa = blok.SvojaAdresa;
                            }
                            else
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode)par.Parent).Right = node;
                                par = (InternalNode)par.Parent;
                                node.Parent = par;
                                node.PocetZaznamov = blok.PocetPlatnychRec;
                                node.Adresa = blok.SvojaAdresa;
                            }
                        }
                        else
                        {
                            if ((par.Right as ExternalNode).PocetZaznamov + node.PocetZaznamov <= PocetVBloku)
                            {
                                Block<T> blok1 = new Block<T>(_zapisovac.citaj((par.Right as ExternalNode).Adresa),_typ );// nacital som bloky aj nevalidne

                                if (blok.SvojaAdresa < blok1.SvojaAdresa)
                                {
                                    for (int i = 0; i < blok1.PocetPlatnychRec; i++)
                                    {
                                        blok.AddRecord(blok1.Records[i]);
                                    }
                                    PridajVolnuAdresu(blok1.SvojaAdresa);
                                    blok1.PocetPlatnychRec = 0;
                                    if (!skratPole(blok1.SvojaAdresa))
                                    {
                                        
                                        _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < blok.PocetPlatnychRec; i++)
                                    {
                                        blok1.AddRecord(blok.Records[i]);
                                    }
                                    blok.PocetPlatnychRec = 0;
                                    PridajVolnuAdresu(blok.SvojaAdresa);
                                    if (!skratPole(blok.SvojaAdresa))
                                    {
                                        
                                        _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                    }

                                    blok = blok1;
                                }
                                if (par.Parent == null)
                                {
                                    Root = node;
                                    node.Parent = null;
                                    node.HlbkaBloku--;
                                    _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                    node.PocetZaznamov = blok.PocetPlatnychRec; ;
                                    node.Adresa = blok.SvojaAdresa;
                                    return;
                                }
                                if (((InternalNode)par.Parent).Left == par)
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode)par.Parent).Left = node;
                                    par = (InternalNode)par.Parent;
                                    node.Parent = par;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                }
                                else
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode)par.Parent).Right = node;
                                    par = (InternalNode)par.Parent;
                                    node.Parent = par;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                }
                            }
                            else
                            {
                                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                return;
                            }
                        }
                    }
                }
            }
        }

        public bool Add(T data)
        {
            if (!Contiens(data))
            {
                Block<T> b = null;
                if (Root == null)
                {
                    Root = NajdiExternalNode(data, ref b);
                    ((ExternalNode)Root).PocetZaznamov++;
                    b.AddRecord(data);
                    _zapisovac.zapis(b.SvojaAdresa, b.ToByteArrays());
                    Console.WriteLine("Vlozil do roota prvy");
                    return true;
                }

                var node = NajdiExternalNode(data, ref b);

                if (node.PocetZaznamov < PocetVBloku)
                {
                    node.PocetZaznamov++;
                    b.AddRecord(data);
                    _zapisovac.zapis(b.SvojaAdresa, b.ToByteArrays());
                    Console.WriteLine("Vlozil cez roota");
                    return true;
                }
                else
                {
                    if (b.PreplnovaciBlok == -1)
                    {
                        var blok = new Block<T>(PocetVBloku, DajAdresu(), _typ);
                        blok.AddRecord(data);
                        b.PreplnovaciBlok = blok.SvojaAdresa;
                        node.PocetZaznamov++;
                        _zapisovac.zapis(b.SvojaAdresa, b.ToByteArrays());
                        _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                        return true;
                    }
                    else
                    {
                        var blok = new Block<T>(_zapisovac.citaj(b.PreplnovaciBlok), _typ);
                        while (true)
                        {
                            if (blok.PocetPlatnychRec < PocetVBloku)
                            {
                                blok.AddRecord(data);
                                node.PocetZaznamov++;
                                _zapisovac.zapis(b.SvojaAdresa, b.ToByteArrays());
                                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                return true;
                            }


                            if (blok.PreplnovaciBlok == -1)
                            {
                                Block<T> blok1 = new Block<T>(PocetVBloku, DajAdresu(), _typ);
                                blok.PreplnovaciBlok = blok1.SvojaAdresa;
                                blok1.AddRecord(data);
                                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
                                node.PocetZaznamov++;
                                return true;
                            }
                            else
                            {
                                b = blok;
                                blok = new Block<T>(_zapisovac.citaj(blok.PreplnovaciBlok), _typ); ;
                            }
                        }
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
