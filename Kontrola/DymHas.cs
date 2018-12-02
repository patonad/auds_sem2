using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace audsSem2
{
    class DymHas<T> where T : IRecord<T>
    {

        private Zapisovac<T> _zapisovac;
        public int Adresa { get; set; }
        public Node Root { get; set; }
        public int PocetVBloku { get; set; }
        private T _typ;
        public List<int> volneBloky;
        public int DlzkaHashu { get; set; }

        public DymHas(int hlbka, T data, int pocetVBloku, string adresaSuboru, int pocetVPrepnlovacomBloku, string adresaPreplnovaciehoSuboru)
        {
            DlzkaHashu = hlbka;
            volneBloky = new List<int>();
            _typ = data;
            _zapisovac = new Zapisovac<T>(adresaSuboru, new Block<T>(2, 0, data));
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
            if (volneBloky.Count() == 0)
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
                if (blok1.PocetPlatnychRec > 0)
                {
                    if (roo.Right == null)
                    {
                        roo.Left = new ExternalNode(roo.HlbkaBloku + 1, ((ExternalNode)node).Adresa);
                        roo.Left.Parent = roo;
                    }
                    else
                    {
                        roo.Left = new ExternalNode(roo.HlbkaBloku + 1, DajAdresu());
                        roo.Left.Parent = roo;
                    }

                    ((ExternalNode)roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
                    blok1.SvojaAdresa = ((ExternalNode)roo.Left).Adresa;
                }
                if (blok2.PocetPlatnychRec > 0)
                {
                    if (roo.Left == null)
                    {
                        roo.Right = new ExternalNode(roo.HlbkaBloku + 1, ((ExternalNode)node).Adresa);
                        roo.Right.Parent = roo;
                    }
                    else
                    {
                        roo.Right = new ExternalNode(roo.HlbkaBloku + 1, DajAdresu());
                        roo.Right.Parent = roo;
                    }
                    ((ExternalNode)roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
                    blok2.SvojaAdresa = ((ExternalNode)roo.Right).Adresa;
                }
                if (AkyBit(node.HlbkaBloku, data.GetHash()) == 0)
                {
                    if (blok1.PocetPlatnychRec < PocetVBloku)
                    {
                        if (blok2.SvojaAdresa != -1)
                        {
                            _zapisovac.zapis(blok2.SvojaAdresa, blok2.ToByteArrays());
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
                        return (ExternalNode)roo.Left;

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

                        node = (ExternalNode)roo.Left;

                    }
                }
                else
                {
                    if (blok2.PocetPlatnychRec < PocetVBloku)
                    {
                        if (blok1.SvojaAdresa != -1)
                        {
                            _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
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
                        return (ExternalNode)roo.Right;
                    }
                    else
                    {
                        blok = blok2;
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
                        node = (ExternalNode)roo.Right;
                        if (node.HlbkaBloku == DlzkaHashu)
                        {
                            return node;
                        }


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
                        return Rozsir(ref blok, data, (ExternalNode)Root);
                    }
                }

            }
            else
            {
                var node = najdiNode(data);
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

        public bool FInd(T data)
        {
            if (Root != null)
            {
                var node = najdiNode(data);
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

        public bool Add(T data)
        {
            if (!FInd(data))
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
                                Block<T> blok1 =  new Block<T>(PocetVBloku, DajAdresu(), _typ);
                                blok.PreplnovaciBlok = blok1.SvojaAdresa;
                                blok1.AddRecord(data);
                                _zapisovac.zapis(blok.SvojaAdresa,blok.ToByteArrays());
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
