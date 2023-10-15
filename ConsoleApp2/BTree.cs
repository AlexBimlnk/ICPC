using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;
public sealed class BTree
{
    private int _rank; // count values
    private Node _root;

    public BTree(int rank)
    {
        _rank = rank;
        _root = new Node(rank);
    }

    public void Add(int value) => _root.Add(value);

    private class Node
    {
        private readonly List<int> _values;
        private readonly List<Node?> _pointers;

        public Node? Parent { get; set; }

        public Node(int rank)
        {
            _values = new List<int>(rank + 1);
            _pointers = new List<Node?>(rank + 2);
            _pointers.AddRange(Enumerable.Repeat((Node)null!, rank + 2));
        }

        public Node(int rank, IEnumerable<int> values, params Node[] pointers) : this(rank)
        {
            _values.AddRange(values);
            for (int i = 0; i < pointers.Length; i++)
            {
                _pointers[i] = pointers[i];
            }
        }

        private int GetSearchableIndex(int value)
        {
            var searchIndex = _values.FindIndex(x => value < x);

            if (searchIndex == -1) // оказались правее всех элементов
            {
                searchIndex = _values.Count;
            }

            return searchIndex;
        }

        private (int BubbleValue, Node LeftNode, Node RightNode) Split()
        {
            var middleIndex = _values.Count / 2;

            var bubbleValue = _values[middleIndex];

            var leftNode = new Node(
                _values.Capacity - 1,
                _values.Take(middleIndex),
                _pointers.Take(middleIndex + 1).ToArray()!);

            var rightNode = new Node(
                _values.Capacity - 1,
                _values.Skip(middleIndex + 1),
                _pointers.Skip(middleIndex + 1).ToArray()!);

            return (bubbleValue, leftNode, rightNode);
        }

        private void SetAsRoot(int bubbleValue, Node leftNode, Node rightNode)
        {
            _values.Clear();
            _values.Add(bubbleValue);

            _pointers[0] = leftNode;
            _pointers[1] = rightNode;

            for (int i = 2; i < _pointers.Count; i++)
            {
                _pointers[i] = null!;
            }

            leftNode.Parent = this;
            rightNode.Parent = this;
        }

        // Пытается добавить без погружения строго на ноду
        private void StrongAdd(int value, Node leftNode, Node rightNode) 
        {
            var searchIndex = GetSearchableIndex(value);

            _values.Insert(searchIndex, value);
            leftNode.Parent = this;
            rightNode.Parent = this;
            _pointers[searchIndex] = leftNode;

            for (int i = _pointers.Count - 1; i >= searchIndex + 2; i--)
            {
                _pointers[i] = _pointers[i - 1];
            }

            _pointers[searchIndex + 1] = rightNode;

            if (_values.Count == _values.Capacity)
            {
                var (bubbleValue, newLeftNode, newRightNode) = Split();

                newLeftNode._pointers.ForEach(x =>
                {
                    if (x != null)
                    {
                        x.Parent = newLeftNode;
                    }
                });

                newRightNode._pointers.ForEach(x =>
                {
                    if (x != null)
                    {
                        x.Parent = newRightNode;
                    }
                });

                if (Parent != null)
                    Parent.StrongAdd(bubbleValue, newLeftNode, newRightNode);
                else
                    SetAsRoot(bubbleValue, newLeftNode, newRightNode);
            }
        }

        public void Add(int value)
        {
            if (!_values.Any())
            {
                _values.Add(value);
                return;
            }
            
            var searchIndex = GetSearchableIndex(value);
            var selectedNode = _pointers[searchIndex];
            if (selectedNode == null) // Если для данного диапазона нет нод
            {
                _values.Insert(searchIndex, value);
                if (_values.Count == _values.Capacity) // Оверсайз в ноде
                {
                    var (bubbleValue, leftNode, rightNode) = Split();

                    if (Parent != null)
                        Parent.StrongAdd(bubbleValue, leftNode, rightNode);
                    else
                        SetAsRoot(bubbleValue, leftNode, rightNode);
                }
            }
            else
                selectedNode.Add(value);
        }
    }
}
