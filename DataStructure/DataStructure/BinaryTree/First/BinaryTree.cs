using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.BinaryTree.First
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
        public Node(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }
    public class BinaryTree<T>
    {        
        private Node<T> head;
        private T[] datas;
        public Node<T> Head
        {
            get
            {
                return head;
            }
        }
        public BinaryTree(T[] vals)
        {
            datas = vals;
            Add(head, 0);
        }
        private void Add(Node<T> parent,int index)
        {
            if(parent==null)
            {
                parent = new Node<T>(datas[index]);
                head = parent;
            }
            int leftIndex = 2 * index + 1;
            int rightIndex = 2 * index + 2;
            if (leftIndex < datas.Length)
            {
                if (!datas[leftIndex].Equals("#"))
                {
                    parent.Left = new Node<T>(datas[leftIndex]);
                    Add(parent.Left, leftIndex);
                }
                else
                {
                    parent.Left = null;
                }
            }
            if (rightIndex < datas.Length)
            {
                if (!datas[rightIndex].Equals("#"))
                {
                    parent.Right = new Node<T>(datas[rightIndex]);
                    Add(parent.Right, rightIndex);
                }
                else
                {
                    parent.Right = null;
                }
            }
        }
        //先序遍历
        public void PreTraversal(Node<T> node)
        {
            if (node != null)
            {
                Console.Write(node.Data + " ");
                PreTraversal(node.Left);
                PreTraversal(node.Right);
            }
        }

        //中序遍历
        public void InTraversal(Node<T> node)
        {
            if (node != null)
            {
                InTraversal(node.Left);
                Console.Write(node.Data + " ");
                InTraversal(node.Right);
            }
        }

        //后序遍历
        public void LastTraversal(Node<T> node)
        {
            if (node != null)
            {
                LastTraversal(node.Left);
                LastTraversal(node.Right);
                Console.Write(node.Data + " ");
            }
        }
        //层次遍历
        //引入队列
        public void LevelTranversal(Node<T> node)
        {
            if (node == null)
            {
                return;
            }
            Queue<Node<T>> queue = new Queue<Node<T>>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                //结点出队
                Node<T> temp = queue.Dequeue();
                Console.Write(temp.Data + " ");
                if (temp.Left != null)
                {
                    queue.Enqueue(temp.Left);
                }
                if (temp.Right != null)
                {
                    queue.Enqueue(temp.Right);
                }
            }
        }
    }
}
