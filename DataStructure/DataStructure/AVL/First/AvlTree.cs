using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructure.AVL.First
{
    public class AVLNode
    {
        public int data; //数据
        public int bF; //平衡因子
        public AVLNode lC; //左孩子
        public AVLNode rC; //右孩子
        public AVLNode parent; //父节点
        public AVLNode(int data)
        {
            this.data = data;
        }
    }
    /// <summary>
    /// 平衡二叉树
    /// </summary>
    public class AVLTree
    {
        private AVLNode root; //根节点
        private int count;
        public int Count { get => count; private set => count = value; }

        /// <summary>
        /// AVL树的插入方法
        /// </summary>
        /// <param name="data"></param>
        public void Insert(int data)
        {
            #region 找到插入点
            if (root == null) //若根节点为空, 则将新节点插入到根节点处
            {
                root = new AVLNode(data);
                return;
            }
            AVLNode parent = root; //表示当前节点的父节点
            AVLNode curNode = root; //表示当前节点
            while (curNode != null)
            {
                parent = curNode;
                if (data < parent.data) //若data小于当前节点, 则当前节点为当前节点的左孩子
                {
                    curNode = parent.lC;
                }
                else
                {
                    curNode = parent.rC;
                }
            }
            //此时已找到空节点
            curNode = new AVLNode(data);
            curNode.parent = parent;
            if (data < parent.data)
            {
                parent.lC = curNode;
            }
            else
            {
                parent.rC = curNode;
            }
            #endregion

            //每插入一个节点便要计算平衡因子
            //平衡因子为左子树高度减去右子树高度
            while (parent != null)
            {
                if (parent.lC == curNode) //若新节点在parent的左节点上, 则平衡因子+1
                {
                    parent.bF++;
                }
                else //若新节点在parent的右节点上, 则平衡因子-1
                {
                    parent.bF--;
                }
                //接下来根据平衡因子判断当前父节点是否平衡
                if (parent.bF == 0)//若parent的平衡因子为0, 则表示当前为平衡状态, 不需要变更
                {
                    break;
                }
                else if (parent.bF == -1 || parent.bF == 1) //若parent的平衡因子为1或-1, 则需要回溯, 验证上一个节点是否平衡
                {
                    curNode = parent;
                    parent = curNode.parent;
                }
                else //若parent的平衡因子为2或-2, 则需要重新平衡二叉树
                {
                    if (parent.bF == 2) //若左子树高于右子树
                    {
                        if (curNode.bF == 1) //LL形
                        {
                            RotateLL(parent); //平衡操作
                        }
                        else //LR形
                        {
                            RotateLR(parent); //平衡操作
                        }
                    }
                    else
                    {
                        if (curNode.bF == -1) //RR形
                        {
                            RotateRR(parent); //平衡操作
                        }
                        else //RL形
                        {
                            RotateRL(parent); //平衡操作
                        }
                    }
                    break;
                }
            }
            count++;
        }
        public void RotateLL(AVLNode parent)
        {
            /*        G                G
             *        丨               丨
             *        P               Cur
             *       / \              /  \
             *     Cur   X1          C     P    
             *     / \              / \   / \
             *    C   X2     ===>  x3 x4 X2 x1   
             *   / \                                
             *  X3  X4                           
             */
            AVLNode grandParent = parent.parent;
            AVLNode curNode = parent.lC;
            if (grandParent != null) //若存在G, 则将cur改为g的孩子
            {
                if (grandParent.lC == parent)
                {
                    grandParent.lC = curNode;
                    curNode.parent = grandParent;
                    grandParent.bF--;
                }
                else
                {
                    grandParent.rC = curNode;
                    curNode.parent = grandParent;
                    grandParent.bF++;
                }
            }
            else //若不存在G
            {
                curNode.parent = null;
                root = curNode;
            }
            parent.lC = curNode.rC; //将cur的右孩子X2改为P的左孩子
            if (parent.lC != null)
            {
                parent.lC.parent = parent; //将P改为X2的parent
            }
            curNode.rC = parent; //将P改为cur的右孩子
            parent.parent = curNode; //将cur改为P的parent
            curNode.bF--;
            parent.bF--;
        }
        public void RotateLR(AVLNode parent)
        {
            /*        G                G
             *        丨               丨
             *        P                C
             *       / \              /  \
             *     Cur   X1         Cur    P    
             *     / \              / \   / \
             *    x2  C      ===>  x2 x3 X4 x1   
             *       / \                                
             *      X3  X4                           
             */
            AVLNode grandParent = parent.parent;
            AVLNode curNode = parent.lC;
            AVLNode c = curNode.rC;
            if (grandParent != null) //若存在G, 则将c改为g的孩子
            {
                if (grandParent.lC == parent)
                {
                    grandParent.lC = c;
                    c.parent = grandParent;
                    grandParent.bF--;
                }
                else
                {
                    grandParent.rC = c;
                    c.parent = grandParent;
                    grandParent.bF++;
                }
            }
            else //若不存在G
            {
                c.parent = null;
                root = c;
            }
            //将cur的右孩子改为x3
            curNode.rC = c.lC;
            if (curNode.rC != null)
            {
                curNode.rC.parent = curNode;
            }
            //将p的左孩子改为X4
            parent.lC = c.rC;
            if (parent.lC != null)
            {
                parent.lC.parent = parent;
            }
            //将c的左孩子改为cur
            c.lC = curNode;
            curNode.parent = c;
            //将c的右孩子改为P
            c.rC = parent;
            parent.parent = c;

            parent.bF = 0;
            curNode.bF++;
            c.bF = curNode.bF;
        }
        public void RotateRR(AVLNode parent)
        {
            /*        G                G
             *        丨               丨
             *        P               Cur
             *       / \              /  \
             *     X1  Cur           P     C    
             *         / \          / \   / \
             *        X2  C  ===>  x1 x2 X3 x4   
             *           / \                                
             *         X3  X4                           
             */
            AVLNode grandParent = parent.parent;
            AVLNode curNode = parent.rC;
            if (grandParent != null) //若存在G, 则将cur改为g的孩子
            {
                if (grandParent.lC == parent)
                {
                    grandParent.lC = curNode;
                    curNode.parent = grandParent;
                    grandParent.bF--;
                }
                else
                {
                    grandParent.rC = curNode;
                    curNode.parent = grandParent;
                    grandParent.bF++;
                }
            }
            else //若不存在G
            {
                curNode.parent = null;
                root = curNode;
            }
            parent.rC = curNode.lC; //将cur的左孩子X2改为P的右孩子
            if (parent.rC != null)
            {
                parent.rC.parent = parent; //将P改为X2的parent
            }
            curNode.lC = parent; //将P改为cur的左孩子
            parent.parent = curNode; //将cur改为P的parent
            curNode.bF++;
            parent.bF++;
        }
        public void RotateRL(AVLNode parent)
        {
            /*        G                G
             *        丨               丨
             *        P                C
             *       / \              /  \
             *      X1 Cur           P    Cur    
             *         / \          / \   / \
             *        C  x2  ===>  x1 x3 X4 x2   
             *       / \                                
             *      X3  X4                           
             */
            AVLNode grandParent = parent.parent;
            AVLNode curNode = parent.rC;
            AVLNode c = curNode.lC;
            if (grandParent != null) //若存在G, 则将c改为g的孩子
            {
                if (grandParent.lC == parent)
                {
                    grandParent.lC = c;
                    c.parent = grandParent;
                    grandParent.bF--;
                }
                else
                {
                    grandParent.rC = c;
                    c.parent = grandParent;
                    grandParent.bF++;
                }
            }
            else //若不存在G
            {
                c.parent = null;
                root = c;
            }
            //将cur的左孩子改为x4
            curNode.lC = c.rC;
            if (curNode.lC != null)
            {
                curNode.lC.parent = curNode;
            }
            //将p的右孩子改为X3
            parent.rC = c.lC;
            if (parent.rC != null)
            {
                parent.rC.parent = parent;
            }
            //将c的右孩子改为cur
            c.rC = curNode;
            curNode.parent = c;
            //将c的左孩子改为P
            c.lC = parent;
            parent.parent = c;

            parent.bF = 0;
            curNode.bF--;
            c.bF = -curNode.bF;
        }

        /// <summary>
        /// 中序遍历
        /// </summary>
        public void InOrder()
        {
            _InOrder(root);
        }
        /// <summary>
        /// 中序遍历(递归)
        /// </summary>
        /// <param name="node"></param>
        private void _InOrder(AVLNode node)
        {
            if (node == null)
            {
                return;
            }
            _InOrder(node.lC);
            Console.Write(node.data + " ");
            _InOrder(node.rC);
        }
    }
}
