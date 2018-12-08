using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
       public string AdresaUlozenia { get; set; }
        public string AdresaSuboru { get; set; }
        public DymHas(int hlbka, T data, int pocetVBloku, string adresaSuboru,
            string adresaUlozenia)
        {
            AdresaSuboru = adresaSuboru;
            AdresaUlozenia = adresaUlozenia;
            DlzkaHashu = hlbka;
            volneBloky = new List<int>();
            _typ = data;
            _zapisovac = new Zapisovac<T>(adresaSuboru, new Block<T>(pocetVBloku, 0, data));
            PocetVBloku = pocetVBloku;
            Adresa = 0;
        }

        //public void UlozSa()
        //{
        //    FileStream fs = new FileStream(AdresaUlozenia,FileMode.Create);
        //    StreamWriter sw = new StreamWriter(fs);
        //    sw.WriteLine(Adresa + ";" + PocetVBloku + ";" + DlzkaHashu + ";" + AdresaUlozenia+";"+AdresaSuboru);
        //    foreach (var i in volneBloky)
        //    {
        //        sw.Write(i+";");
        //    }
        //    if (Root == null)
        //    {
        //        return;
        //    }
        //    Queue<Node> queue = new Queue<Node>();
        //    queue.Enqueue(Root);
        //    int thisLevel = 1;
        //    int nextLevel = 0;
        //    Node node;
        //    while (queue.Any())
        //    {
        //        for (int i = 0; i < thisLevel; i++)
        //        {
        //            node = queue.Dequeue();
        //            traversal.append(node.data); // prida
        //            if (node.left != null && node.right != null)
        //            {
        //                nextLevel += 2;
        //                queue.add(node.left);
        //                queue.add(node.right);
        //            }
        //            else if (node.left != null || node.right != null)
        //            {
        //                nextLevel += 1;
        //                if (node.left != null)
        //                {
        //                    queue.add(node.left);
        //                }
        //                else
        //                {
        //                    queue.add(node.right);
        //                }
        //            }
        //        }
        //        thisLevel = nextLevel;
        //        nextLevel = 0;
        //    }
        //    return traversal.toString();
        //}

        public int AkyBit(int pozicia, byte[] pole)
        {
            if (pozicia >= pole.Length*8 || pozicia == DlzkaHashu)
            {
                throw new Exception("Zla Hashovacka");
            }
                return (pole[pozicia / 8] & (1 << pozicia % 8)) != 0 ? 1 : 0;
          
        }
        public void PridajVolnuAdresu(int data)
        {
            
            if (volneBloky.Count == 0)
            {
                volneBloky.Add(data);
            }
            else
            {
                int minimum = 0;
                int maximum = volneBloky.Count;
                int index;
                while (true)
                {
                    int akt = (maximum + minimum) / 2;
                    int adr = volneBloky[akt];
                    if (adr < data)
                    {
                        if (akt < volneBloky.Count - 1)
                        {
                            minimum = akt + 1;
                        }
                        else
                        {
                            index = volneBloky.Count;
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
                volneBloky.Insert(index,data);

            }
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
                int a = volneBloky[0];
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
                    node = ((InternalNode) node).Left;
                }
                else
                {
                    node = ((InternalNode) node).Right;
                }

            }

            return (ExternalNode) node;
        }

        public bool skratPole(int adresa)
        {
            bool ret = false;
            if (volneBloky.Count != 0)
            {
                int a = 0;
                while (volneBloky.Last() == Adresa - 1)
                {
                    volneBloky.Remove(volneBloky.Last());
                    Adresa--;
                    a++;
                    if (volneBloky.Count == 0)
                    {
                      break;
                    }
                }
                if (a != 0)
                {
                    _zapisovac.skrat(a);
                    ret = true;
                }
            }
            return ret;
        }

        public void zatvot()
        {
            _zapisovac.Zatvor();
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
                roo.Right = new ExternalNode(roo.HlbkaBloku + 1, -1);
                roo.Right.Parent = roo;

                if (AkyBit(node.HlbkaBloku, data.GetHash()) == 0) //pasnw tu
                {
                    if (blok1.PocetPlatnychRec < PocetVBloku)
                    {
                        if (blok2.PocetPlatnychRec != 0)
                        {
                            if (((ExternalNode) roo.Left).Adresa == -1)
                            {
                                ((ExternalNode) roo.Right).Adresa = node.Adresa;
                                blok2.SvojaAdresa = ((ExternalNode) roo.Right).Adresa;
                                ((ExternalNode) roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
                                _zapisovac.zapis(blok2.SvojaAdresa, blok2.ToByteArrays());
                            }
                            else
                            {
                                ((ExternalNode) roo.Right).Adresa = DajAdresu();
                                blok2.SvojaAdresa = ((ExternalNode) roo.Right).Adresa;
                                ((ExternalNode) roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
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
                            if (((InternalNode) node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Right = roo;
                            }
                        }

                        if (((ExternalNode) roo.Right).Adresa == -1)
                        {
                            ((ExternalNode) roo.Left).Adresa = node.Adresa;
                            blok1.SvojaAdresa = ((ExternalNode) roo.Left).Adresa;
                        }
                        else
                        {
                            ((ExternalNode) roo.Left).Adresa = DajAdresu();
                            blok1.SvojaAdresa = ((ExternalNode) roo.Left).Adresa;
                        }

                        roo.Left.Parent = roo;
                        ((ExternalNode) roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
                        return (ExternalNode) roo.Left; //tuto
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
                            if (((InternalNode) node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Right = roo;
                            }
                        }

                        var a = node.Adresa;
                        node = (ExternalNode) roo.Left;
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
                            if (((ExternalNode) roo.Right).Adresa == -1)
                            {
                                ((ExternalNode) roo.Left).Adresa = node.Adresa;
                                blok1.SvojaAdresa = ((ExternalNode) roo.Left).Adresa;
                                ((ExternalNode) roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
                                _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
                            }
                            else
                            {
                                ((ExternalNode) roo.Left).Adresa = DajAdresu();
                                blok1.SvojaAdresa = ((ExternalNode) roo.Left).Adresa;
                                ((ExternalNode) roo.Left).PocetZaznamov = blok1.PocetPlatnychRec;
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
                            if (((InternalNode) node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Right = roo;
                            }
                        }

                        if (((ExternalNode) roo.Left).Adresa == -1)
                        {
                            ((ExternalNode) roo.Right).Adresa = node.Adresa;
                            blok2.SvojaAdresa = ((ExternalNode) roo.Right).Adresa;
                        }
                        else
                        {
                            ((ExternalNode) roo.Right).Adresa = DajAdresu();
                            blok2.SvojaAdresa = ((ExternalNode) roo.Right).Adresa;
                        }

                        roo.Right.Parent = roo;
                        ((ExternalNode) roo.Right).PocetZaznamov = blok2.PocetPlatnychRec;
                        return (ExternalNode) roo.Right; //tuto
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
                            if (((InternalNode) node.Parent).Left == node)
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Left = roo;
                            }
                            else
                            {
                                roo.Parent = node.Parent;
                                ((InternalNode) node.Parent).Right = roo;
                            }
                        }

                        var a = node.Adresa;
                        node = (ExternalNode) roo.Right;
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
                if (((ExternalNode) Root).PocetZaznamov < PocetVBloku)
                {
                    blok = new Block<T>(_zapisovac.citaj(((ExternalNode) Root).Adresa), _typ, ((ExternalNode)Root).Adresa);
                    return (ExternalNode) Root;
                }
                else
                {
                    blok = new Block<T>(_zapisovac.citaj(((ExternalNode) Root).Adresa), _typ, ((ExternalNode)Root).Adresa);
                    //if (Root.HlbkaBloku == DlzkaHashu)
                    //{
                    //    return (ExternalNode) Root;
                    //}
                    //else
                    //{
                        var a = Rozsir(ref blok, data, (ExternalNode) Root);
                        if (a.Adresa == -1)
                        {
                            blok = new Block<T>(PocetVBloku, DajAdresu(), _typ);
                            a.Adresa = blok.SvojaAdresa;
                        }

                        return a;
                    //}
                }
            }
            else
            {
                var node = najdiNode(data);
                if (node.Adresa == -1)
                {

                    node.Adresa = DajAdresu();
                    blok = new Block<T>(PocetVBloku, node.Adresa, _typ);
                    return node;
                }

                if (node.PocetZaznamov < PocetVBloku)
                {
                    blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ, node.Adresa);
                    return node;
                }
                else
                {
                    blok = new Block<T>(_zapisovac.citaj(((ExternalNode) node).Adresa), _typ, ((ExternalNode)node).Adresa);
                    //if (node.HlbkaBloku == DlzkaHashu)
                    //{
                    //    return (ExternalNode) node;
                    //}
                    //else
                    //{
                        node = Rozsir(ref blok, data, node);
                        return node;
                    //
                    //}
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

                blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ, node.Adresa);
                while (true)
                {
                    foreach (var blokRecord in blok.Records)
                    {
                        if (blokRecord.Equals(data))
                        {
                            return node;
                        }
                    }
                        return null;
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

                var blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ, node.Adresa);
                while (true)
                {
                    foreach (var blokRecord in blok.Records)
                    {
                        if (blokRecord.Equals(data))
                        {
                            return blokRecord;
                        }
                    }

                    
                        return default(T);
                    

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

                var blok = new Block<T>(_zapisovac.citaj(node.Adresa), _typ, node.Adresa);
                while (true)
                {
                    foreach (var blokRecord in blok.Records)
                    {
                        if (blokRecord.Equals(data))
                        {
                            return true;
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
                var blok = new Block<T>(pole, _typ,i);
                ret.Add(blok);
            }

            return ret;
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

                InternalNode par = (InternalNode) node.Parent;
                // ci som lavz alebo pravy
                while (true)
                {
                    if (par.Right == node)
                    {
                        if (par.Left is InternalNode)
                        {
                            if (blok.PocetPlatnychRec == 0)
                            {
                                node.Adresa = -1;
                                PridajVolnuAdresu(blok.SvojaAdresa);
                                if (!skratPole(blok.SvojaAdresa))
                                {
                                    _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                    return;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                            return;
                        }

                        if (((ExternalNode) par.Left).Adresa == -1)
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

                            if (((InternalNode) par.Parent).Left == par)
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode) par.Parent).Left = node;
                                par = (InternalNode) par.Parent;
                                node.Parent = par;
                            }
                            else
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode) par.Parent).Right = node;
                                par = (InternalNode) par.Parent;
                                node.Parent = par;
                            }
                        }
                        else
                        {
                            if ((par.Left as ExternalNode).PocetZaznamov + node.PocetZaznamov <= PocetVBloku)
                            {
                                Block<T> blok1 = new Block<T>(_zapisovac.citaj((par.Left as ExternalNode).Adresa),
                                    _typ, (par.Left as ExternalNode).Adresa); // nacital som bloky aj nevalidne
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

                                if (((InternalNode) par.Parent).Left == par)
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode) par.Parent).Left = node;
                                    par = (InternalNode) par.Parent;
                                    node.Parent = par;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                }
                                else
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode) par.Parent).Right = node;
                                    par = (InternalNode) par.Parent;
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
                    else
                    {
                        if (par.Right is InternalNode)
                        {
                            if (blok.PocetPlatnychRec == 0)
                            {
                                node.Adresa = -1;
                                PridajVolnuAdresu(blok.SvojaAdresa);
                                if (!skratPole(blok.SvojaAdresa))
                                {
                                    _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                                    return;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                            return;
                        }

                        if (((ExternalNode) par.Right).Adresa == -1)
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

                            if (((InternalNode) par.Parent).Left == par)
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode) par.Parent).Left = node;
                                par = (InternalNode) par.Parent;
                                node.Parent = par;
                                node.PocetZaznamov = blok.PocetPlatnychRec;
                                node.Adresa = blok.SvojaAdresa;
                            }
                            else
                            {
                                node.HlbkaBloku = par.HlbkaBloku;
                                ((InternalNode) par.Parent).Right = node;
                                par = (InternalNode) par.Parent;
                                node.Parent = par;
                                node.PocetZaznamov = blok.PocetPlatnychRec;
                                node.Adresa = blok.SvojaAdresa;
                            }
                        }
                        else
                        {
                            if ((par.Right as ExternalNode).PocetZaznamov + node.PocetZaznamov <= PocetVBloku)
                            {
                                Block<T> blok1 = new Block<T>(_zapisovac.citaj((par.Right as ExternalNode).Adresa),
                                    _typ, (par.Right as ExternalNode).Adresa); // nacital som bloky aj nevalidne

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
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                    return;
                                }

                                if (((InternalNode) par.Parent).Left == par)
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode) par.Parent).Left = node;
                                    par = (InternalNode) par.Parent;
                                    node.Parent = par;
                                    node.PocetZaznamov = blok.PocetPlatnychRec;
                                    node.Adresa = blok.SvojaAdresa;
                                }
                                else
                                {
                                    node.HlbkaBloku = par.HlbkaBloku;
                                    ((InternalNode) par.Parent).Right = node;
                                    par = (InternalNode) par.Parent;
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
                    ((ExternalNode) Root).PocetZaznamov++;
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
                    return true;
                }
                //else
                //{
                //    if (b.PreplnovaciBlok == -1)
                //    {
                //        var blok = new Block<T>(PocetVBloku, DajAdresu(), _typ);
                //        blok.AddRecord(data);
                //        b.PreplnovaciBlok = blok.SvojaAdresa;
                //        node.PocetZaznamov++;
                //        _zapisovac.zapis(b.SvojaAdresa, b.ToByteArrays());
                //        _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                //        return true;
                //    }
                //    else
                //    {
                //        var blok = new Block<T>(_zapisovac.citaj(b.PreplnovaciBlok), _typ);
                //        while (true)
                //        {
                //            if (blok.PocetPlatnychRec < PocetVBloku)
                //            {
                //                blok.AddRecord(data);
                //                node.PocetZaznamov++;
                //                _zapisovac.zapis(b.SvojaAdresa, b.ToByteArrays());
                //                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                //                return true;
                //            }


                //            if (blok.PreplnovaciBlok == -1)
                //            {
                //                Block<T> blok1 = new Block<T>(PocetVBloku, DajAdresu(), _typ);
                //                blok.PreplnovaciBlok = blok1.SvojaAdresa;
                //                blok1.AddRecord(data);
                //                _zapisovac.zapis(blok.SvojaAdresa, blok.ToByteArrays());
                //                _zapisovac.zapis(blok1.SvojaAdresa, blok1.ToByteArrays());
                //                node.PocetZaznamov++;
                //                return true;
                //            }
                //            else
                //            {
                //                b = blok;
                //                blok = new Block<T>(_zapisovac.citaj(blok.PreplnovaciBlok), _typ); ;
                //            }
                //        }
                //    }
                //    return false;
                //}
            }
            else
            {
               throw new Exception("Zly hash");
            }

            return false;
        }
    }
}

