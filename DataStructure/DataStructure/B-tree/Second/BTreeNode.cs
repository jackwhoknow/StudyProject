using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.B_tree.Second
{
    public class BTreeNode
    {
        private bool leaf;
        public int[] keys;
        public int keyNumber;
        public BTreeNode[] children;
        public int blockIndex;
        public int dataIndex;

        public BTreeNode(bool leaf)
        {
            this.leaf = leaf;
            keys = new int[Consts.KeyMax];
            children = new BTreeNode[Consts.ChildMax];
        }

        /// <summary>在未满的节点中插入键值</summary>
        /// <param name="key">键值</param>
        public void InsertNonFull(int key)
        {
            var index = keyNumber - 1;

            if (leaf == true)
            {
                // 找到合适位置,并且移动节点键值腾出位置
                while (index >= 0 && keys[index] > key)
                {
                    keys[index + 1] = keys[index];
                    index--;
                }

                // 在index后边新增键值
                keys[index + 1] = key;
                keyNumber = keyNumber + 1;
            }
            else
            {
                // 找到合适的子孩子索引
                while (index >= 0 && keys[index] > key) index--;

                // 如果孩子节点已满
                if (children[index + 1].keyNumber == Consts.KeyMax)
                {
                    // 分裂该孩子节点
                    SplitChild(index + 1, children[index + 1]);

                    // 分裂后中间节点上跳父节点
                    // 孩子节点已经分裂成2个节点,找到合适的一个
                    if (keys[index + 1] < key) index++;
                }

                // 插入键值
                children[index + 1].InsertNonFull(key);
            }
        }

        /// <summary>分裂节点</summary>
        /// <param name="childIndex">孩子节点索引</param>
        /// <param name="waitSplitNode">待分裂节点</param>
        public void SplitChild(int childIndex, BTreeNode waitSplitNode)
        {
            var newNode = new BTreeNode(waitSplitNode.leaf);
            newNode.keyNumber = Consts.KeyMin;

            // 把待分裂的节点中的一般节点搬到新节点
            for (var j = 0; j < Consts.KeyMin; j++)
            {
                newNode.keys[j] = waitSplitNode.keys[j + Consts.ChildMin];

                // 清0
                waitSplitNode.keys[j + Consts.ChildMin] = 0;
            }

            // 如果待分裂节点不是也只节点
            if (waitSplitNode.leaf == false)
            {
                for (var j = 0; j < Consts.ChildMin; j++)
                {
                    // 把孩子节点也搬过去
                    newNode.children[j] = waitSplitNode.children[j + Consts.ChildMin];

                    // 清0
                    waitSplitNode.children[j + Consts.ChildMin] = null;
                }
            }

            waitSplitNode.keyNumber = Consts.KeyMin;

            // 拷贝一般键值到新节点
            for (var j = keyNumber; j >= childIndex + 1; j--)
                children[j + 1] = children[j];

            children[childIndex + 1] = newNode;
            for (var j = keyNumber - 1; j >= childIndex; j--)
                keys[j + 1] = keys[j];

            // 把中间键值上跳至父节点
            keys[childIndex] = waitSplitNode.keys[Consts.KeyMin];

            // 清0
            waitSplitNode.keys[Consts.KeyMin] = 0;

            // 根节点键值数自加
            keyNumber = keyNumber + 1;
        }

        /// <summary>根据节点索引顺序打印节点键值</summary>
        public void PrintByIndex()
        {
            int index;
            for (index = 0; index < keyNumber; index++)
            {
                // 如果不是叶子节点, 先打印叶子子节点. 
                if (leaf == false) children[index].PrintByIndex();

                Console.Write("{0} ", keys[index]);
            }

            // 打印孩子节点
            if (leaf == false) children[index].PrintByIndex();
        }

        /// <summary>查找某键值是否已经存在树中</summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public BTreeNode Find(int key)
        {
            int index = 0;
            while (index < keyNumber && key > keys[index]) index++;

            // 该key已经存在, 返回该索引位置节点
            if (keys[index] == key) return this;

            // key 不存在,并且节点是叶子节点
            if (leaf == true) return null;

            // 递归在孩子节点中查找
            return children[index].Find(key);
        }
    }
}
