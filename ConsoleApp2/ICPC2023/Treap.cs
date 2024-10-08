using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2023;

public class Treap
{
    private Random _random = new Random();
    private Node _root;

    public Treap()
    {
    }

    private Node Merge(Node left, Node right)
    {
        if (left == null)
            return right;
        if (right == null)
            return left;

        if (left.Y < right.Y)
        {
            left.Right = Merge(left.Right, right);
            return left;
        }
        else
        {
            right.Left = Merge(left, right.Left);
            return right;
        }
    }

    private (Node Left, Node Right) Split(Node root, int x)
    {
        if (root == null)
            return (null, null)!;

        if (root.X < x)
        {
            var (leftNode, rightNode) = Split(root.Right, x);
            root.Right = leftNode;

            return (root, rightNode);
        }
        else
        {
            var (leftNode, rightNode) = Split(root.Left, x);
            root.Left = rightNode;

            return (leftNode, root);
        }
    }

    public void Add(int x)
    {
        var node = new Node(x, _random.Next());

        if (_root == null)
            _root = node;
        else
        {
            var (left, right) = Split(_root, x);

            _root = Merge(left, Merge(node, right));
        }
    }

    public void Remove(int x)
    {
        var (left, right) = Split(_root, x + 1);
        var (left2, _) = Split(left, x);

        _root = Merge(left2, right);
    }

    private class Node
    {
        public int X { get; }
        public int Y { get; }

        public Node Parent { get; set; }

        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
