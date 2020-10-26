using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.B_tree.First
{
    public class TreeNode<T> where T:IComparable<T>
    {
        public int elementNum;
        public IList<T> Elements = new List<T>(); //元素集合,存在elementNum个
        public IList<TreeNode<T>> Pointer = new List<TreeNode<T>>(); //元素指针，存在elementNum+1
        public bool IsLeaf = true; //是否为叶子节点
    }
}
