using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.B_tree.Second
{
    public class Consts
    {
        public const int M = 3;                  // B树的最小度数
        public const int KeyMax = 2 * M - 1;     // 节点包含关键字的最大个数
        public const int KeyMin = M - 1;         // 非根节点包含关键字的最小个数
        public const int ChildMax = KeyMax + 1;  // 孩子节点的最大个数
        public const int ChildMin = KeyMin + 1;  // 孩子节点的最小个数
    }
}
