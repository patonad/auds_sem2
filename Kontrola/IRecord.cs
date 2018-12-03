using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace audsSem2
{
    public interface IRecord<T>
    {
        byte[] GetHash();
        bool Equals(T rec);
        byte[] ToByArray();
        T FromByArray(byte[] pole);
        int GetSize();
        T DuplicujSA(byte[] data);
    }
}