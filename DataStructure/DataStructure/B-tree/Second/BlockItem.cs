using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.B_tree.Second
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct BlockItem
    {
        public int ChildBlockIndex;
        public int Key;
        public int DataIndex;
        public BlockItem(int key,int dataIndex)
        {
            ChildBlockIndex = -1;
            Key = key;
            DataIndex = dataIndex;
        }
    }
}
