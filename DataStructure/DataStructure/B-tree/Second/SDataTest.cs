using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.B_tree.Second
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SDataTest
    {
        public int Idx;
        public int Age;
        public byte Sex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Name;
        public byte Valid;
    }
}
