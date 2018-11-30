using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace audsSem2
{
    class DymHas
    {
        public DymHas(int pocet)
        {
            PocetVBloku = pocet;
            Adresa = 0;
        }

        public int AkyBit(int pozicia, byte[] pole)
        {
            return (pole[pozicia / 8] & (1 << pozicia % 8)) != 0 ? 1 : 0;
        }

        public int Adresa { get; set; }
        public Node Root { get; set; }

        public int PocetVBloku { get; set; }
        public int Add(byte[] pole)
        {

            if (Root == null)
            {
                Root = new ExternalNode(0);
                ((ExternalNode)Root).Pridaj(pole);
                Console.WriteLine("Vlozil do roota prvy");
                return Adresa;
            }
            if (Root is ExternalNode)
            {
                if (((ExternalNode)Root).PocetZaznamov < PocetVBloku)
                {
                    ((ExternalNode)Root).Pridaj(pole);
                    Console.WriteLine("Vlozil do roota dalsi");
                    return Adresa;
                }
                else
                {
                    Console.WriteLine("Rozsirujem sa");
                    var inter = new InternalNode(Root.HlbkaBloku);
                    foreach (var zaznam in ((ExternalNode)Root).pole)
                    {
                        if (AkyBit(Root.HlbkaBloku, zaznam) == 0)
                        {
                            if (inter.Left == null)
                            {
                                inter.Left = new ExternalNode(Root.HlbkaBloku + 1);
                            }
                            ((ExternalNode)((InternalNode)inter).Left).Pridaj(zaznam);
                            Console.WriteLine("Prehadzujem lavy");
                        }
                        else
                        {
                            if (inter.Right == null)
                            {
                                inter.Right = new ExternalNode(Root.HlbkaBloku + 1);
                            }
                            ((ExternalNode)((InternalNode)inter).Right).Pridaj(zaznam);
                            Console.WriteLine("Prehadzujem pravy");
                        }
                    }

                    Root = inter;
                }
            }
            var node = Root;
            var poNode = node;
            while (node is InternalNode)
            {
                //traverzuje po externy
                if (AkyBit(node.HlbkaBloku, pole) == 0)//tuto
                {
                    if (((InternalNode)node).Left != null)
                    {
                        poNode = node;
                        node = ((InternalNode) node).Left;
                    }
                    else
                    {
                        ((InternalNode)node).Left = new ExternalNode(node.HlbkaBloku);
                        Console.WriteLine("Vytvaram lavy");
                    }
                }
                else
                {
                    if (((InternalNode)node).Right != null)
                    {
                        poNode = node;
                        node = ((InternalNode)node).Right;
                    }
                    else
                    {
                        ((InternalNode)node).Right = new ExternalNode(node.HlbkaBloku);
                        Console.WriteLine("Vytvaram pravy");
                    }
                }
                //pridava
                if (node is ExternalNode)
                {
                    if (((ExternalNode)node).PocetZaznamov < PocetVBloku)
                    {
                        ((ExternalNode)node).Pridaj(pole);
                        Console.WriteLine("Vlozil do ex dalsi");
                        return Adresa;
                    }
                    else
                    {
                        Console.WriteLine("Rozsirujem sa");
                       

                        var inter = new InternalNode(node.HlbkaBloku);
                        foreach (var zaznam in ((ExternalNode)node).pole)
                        {
                            if (AkyBit(node.HlbkaBloku, zaznam) == 0)
                            {
                                if (inter.Left == null)
                                {
                                    inter.Left = new ExternalNode(node.HlbkaBloku + 1);
                                }
                                ((ExternalNode)((InternalNode)inter).Left).Pridaj(zaznam);
                                Console.WriteLine("Prehadzujem lavy");
                            }
                            else
                            {
                                if (inter.Right == null)
                                {
                                    inter.Right = new ExternalNode(node.HlbkaBloku + 1);
                                }
                                ((ExternalNode)((InternalNode)inter).Right).Pridaj(zaznam);
                                Console.WriteLine("Prehadzujem pravy");
                            }
                        }

                        if (((InternalNode)poNode).Left == node)
                        {
                            ((InternalNode) poNode).Left = inter;
                            node = inter;
                        }
                        else{
                            ((InternalNode)poNode).Right = inter;
                            node = inter;
                        }
                    }
                }


            }

            return 1;
        }
    }
}
