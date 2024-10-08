using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2023.Problems;
internal class E_Problem_SegTree_Training
{
    private class SegTree
    {
        public struct Node
        {
            public int MaxValue { get; set; }
            public int OriginalIndexMax { get; set; }
            public int MinValue { get; set; }
            public int OriginalIndexMin { get; set; }
        }

        private readonly int _size;
        private readonly Node[] _tree;

        public SegTree(IReadOnlyList<int> ints)
        {
            _size = ints.Count;
            _tree = new Node[2 * _size];
            Build(ints);
        }

        private Node CreateNode(int current)
        {
            var leftChild = _tree[current * 2];
            var rightChild = _tree[current * 2 + 1];

            var maxNode = leftChild.MaxValue > rightChild.MaxValue
                ? leftChild
                : rightChild;

            var minNode = leftChild.MinValue < rightChild.MinValue
                ? leftChild
                : rightChild;

            return new Node
            {
                MaxValue = maxNode.MaxValue,
                OriginalIndexMax = maxNode.OriginalIndexMax,
                MinValue = minNode.MinValue,
                OriginalIndexMin = minNode.OriginalIndexMin
            };
        }

        private void Build(IReadOnlyList<int> ints)
        {
            for (int i = _size; i < _tree.Length; i++)
            {
                _tree[i] = new Node
                {
                    MaxValue = ints[i - _size],
                    OriginalIndexMax = i - _size,

                    MinValue = ints[i - _size],
                    OriginalIndexMin = i - _size
                };
            }

            for (int i = _size - 1; i > 0; i--)
            {
                _tree[i] = CreateNode(i);
            }
        }

        private void UpdateTree(int index, int value)
        {
            var treeIndex = index + _size;

            var node = _tree[treeIndex];
            node.MaxValue = value;
            node.MinValue = value;

            _tree[treeIndex] = node;

            for (int i = treeIndex / 2; i > 0; i /= 2)
            {
                _tree[i] = CreateNode(i);
            }
        }

        private Node GetMinMax(int leftIndex, int rigthIndex)
        {
            var result = new Node
            {
                MaxValue = int.MinValue,
                OriginalIndexMax = -1,
                MinValue = int.MaxValue,
                OriginalIndexMin = -1,
            };

            var lPointer = leftIndex + _size;
            var rPointer = rigthIndex + _size;

            var possibleNodes = new List<Node>();

            for (; lPointer <= rPointer; lPointer /= 2, rPointer /= 2)
            {
                // Если левый указатель указывает на правого потомка
                if (lPointer % 2 == 1)
                    possibleNodes.Add(_tree[lPointer++]);

                // Если правый указатель указывает на левого потомка
                if (rPointer % 2 == 0)
                    possibleNodes.Add(_tree[rPointer--]);
            }

            for (int i = 0; i < possibleNodes.Count; i++)
            {
                if (result.MaxValue < possibleNodes[i].MaxValue)
                {
                    result.MaxValue = possibleNodes[i].MaxValue;
                    result.OriginalIndexMax = possibleNodes[i].OriginalIndexMax;
                }
                if (result.MinValue > possibleNodes[i].MinValue)
                {
                    result.MinValue = possibleNodes[i].MinValue;
                    result.OriginalIndexMin = possibleNodes[i].OriginalIndexMin;
                }
            }

            return result;
        }

        public void Update(int leftIndex, int rigthIndex)
        {
            var minAndMax = GetMinMax(leftIndex, rigthIndex);

            UpdateTree(minAndMax.OriginalIndexMax, minAndMax.MinValue);
            UpdateTree(minAndMax.OriginalIndexMin, minAndMax.MaxValue);
        }

        public void PrintState()
        {
            Console.WriteLine(string.Join(' ', _tree[_size..].Select(x => x.MinValue)));
        }
    }

    public static void Start()
    {
        // Шакти
        var input = Console.ReadLine()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        var q = input[1];

        var data = Console.ReadLine()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        var segTree = new SegTree(data);

        for (int i = 0; i < q; i++)
        {
            var segment = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            var left = segment[0];
            var right = segment[1];

            segTree.Update(left - 1, right - 1);
        }

        segTree.PrintState();
    }
}
