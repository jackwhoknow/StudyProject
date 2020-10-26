using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.B_tree.Second
{
    public class BTree
    {
        public BTreeNode Root { get; private set; }

        public BTree() { }

        /// <summary>根据节点索引顺序打印节点键值</summary>
        public void PrintByIndex()
        {
            if (Root == null)
            {
                Console.WriteLine("空树");
                return;
            }

            Root.PrintByIndex();
        }

        /// <summary>查找某键值是否已经存在树中</summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public BTreeNode Find(int key)
        {
            if (Root == null) return null;

            return Root.Find(key);
        }

        /// <summary>新增B树节点键值</summary>
        /// <param name="key">键值</param>
        public void Insert(int key)
        {
            if (Root == null)
            {
                Root = new BTreeNode(true);
                Root.keys[0] = key;
                Root.keyNumber = 1;
                return;
            }

            if (Root.keyNumber == Consts.KeyMax)
            {
                var newNode = new BTreeNode(false);

                newNode.children[0] = Root;
                newNode.SplitChild(0, Root);

                var index = 0;
                if (newNode.keys[0] < key) index++;

                newNode.children[index].InsertNonFull(key);
                Root = newNode;
            }
            else
            {
                Root.InsertNonFull(key);
            }
        }
    }
}
