using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleApp2.ICPC2023;

/// <summary>
/// Двоичное дерево поиска.
/// </summary>
public sealed class BinarySearchTree
{
    private Node _root;

    public void Add(int value)
    {
        if (_root == null)
            _root = new Node(value);
        else
            _root.Add(value);
    }

    public Node Find(int value)
    {
        Node temp = _root;

        while (temp != null && temp.Value != value)
        {
            if (value < temp.Value)
                temp = temp.LeftChild;

            else
                temp = temp.RightChild;
        }

        return temp!;
    }

    public void Remove(int data)
    {
        Node node = Find(data);
        if (node == null)
            return;

        //Если у узла нет дочерних элементов
        if (node.LeftChild == null && node.RightChild == null)
        {
            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = null;
            else
                node.Parent.RightChild = null;
        }

        //Если нет левого дочернего
        else if (node.LeftChild == null)
        {
            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = node.RightChild;
            else
                node.Parent.RightChild = node.RightChild;

            node.RightChild.Parent = node.Parent;
        }

        //Если нет правого дочернего
        else if (node.RightChild == null)
        {
            if (node.Parent.LeftChild == node)
                node.Parent.LeftChild = node.LeftChild;
            else
                node.Parent.RightChild = node.LeftChild;

            node.LeftChild.Parent = node.Parent;
        }

        else
        {
            Node temp = node.LeftChild;

            while (temp.RightChild != null)
                temp = temp.RightChild;

            if (temp.LeftChild != null)
            {
                temp.Parent.RightChild = temp.LeftChild;
                temp.LeftChild.Parent = temp.Parent;
            }

            node.Value = temp.Value;
        }
    }


    public sealed class Node
    {
        public int Value { get; set; }

        public Node Parent { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }

        public Node(int value)
        {
            Value = value;
        }

        public Node(int value, Node parent) : this(value)
        {
            Parent = parent;
        }

        public void Add(int value)
        {
            if (value < Value)
            {
                if (LeftChild == null)
                    LeftChild = new Node(value, this);
                else
                    LeftChild.Add(value);
            }
            else
            {
                if (RightChild == null)
                    RightChild = new Node(value, this);
                else
                    RightChild.Add(value);
            }
        }
    }
}
