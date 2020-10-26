using System;

namespace DataStructure
{
    class Program
    {
        static void Main(string[] args)
        {
            #region-----------AVL Tree----------
            //AVL.First.AVLTree aVLTreeFirst = new AVL.First.AVLTree();
            //aVLTreeFirst.Insert(9);
            //aVLTreeFirst.Insert(8);
            //aVLTreeFirst.Insert(7);
            //aVLTreeFirst.Insert(6);
            //aVLTreeFirst.Insert(5);
            //aVLTreeFirst.Insert(4);
            //aVLTreeFirst.Insert(3);
            //aVLTreeFirst.Insert(2);
            //aVLTreeFirst.Insert(1);
            //aVLTreeFirst.InOrder();

            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //var aVLTreeSecond = new AVL.Second.AVLTree<int>();
            //for (int i = 1; i <= 100000; i++)
            //{
            //    aVLTreeSecond.Insert(i);
            //}
            //stopwatch.Stop();
            //Console.WriteLine("耗时：" + stopwatch.Elapsed.TotalSeconds);
            //var minValue= aVLTreeSecond.FindMin();
            //var maxValue = aVLTreeSecond.FindMax();
            //bool isFind = aVLTreeSecond.Contains(5);
            #endregion

            #region-----------BinaryTree----------
            //int[] datas = new int[] { 12, 13, 14, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 10, 11 };
            //BinaryTree.First.BinaryTree<int> binaryTree = new BinaryTree.First.BinaryTree<int>(datas);
            //binaryTree.LastTraversal(binaryTree.Head);
            #endregion

            #region-----------B-Tree----------
            //DataStructure.B_tree.First.BTree<int> bTree = new B_tree.First.BTree<int>();
            //for(int i =0;i<3;i++)
            //{
            //    bTree.BtreeInsert(i);
            //}

            var bTree = new DataStructure.B_tree.Second.BTree();
            //bTree.Insert(4);
            //bTree.Insert(5);
            //bTree.Insert(6);
            //bTree.Insert(1);
            //bTree.Insert(2);
            //bTree.Insert(3);
            //bTree.Insert(10);
            //bTree.Insert(11);
            //bTree.Insert(12);
            //bTree.Insert(7);
            //bTree.Insert(8);
            //bTree.Insert(9);
            //bTree.Insert(13);
            //bTree.Insert(14);
            //bTree.Insert(18);
            //bTree.Insert(19);
            //bTree.Insert(20);
            //bTree.Insert(15);
            //bTree.Insert(16);
            //bTree.Insert(17);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i=1;i<=100;i++)
            {
                bTree.Insert(i);
            }
            stopwatch.Stop();
            Console.WriteLine("耗时：" + stopwatch.Elapsed.TotalSeconds);
            Console.WriteLine("输出排序后键值");
            //bTree.PrintByIndex();
            #endregion
            Console.ReadKey();
        }
    }
}
