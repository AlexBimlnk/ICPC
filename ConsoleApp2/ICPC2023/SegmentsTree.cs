using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2023;

/// <summary>
/// Дерево отрезков.
/// </summary>
public sealed class SegmentsTree
{
    /// <summary>
    /// Дерево отрезков для поиска сумм на отрезках.
    /// </summary>
    public static void SegmentsTreeOfSum()
    {
        var input = Console.ReadLine()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        var segTree = new SegTree(input);


    }

    private sealed class SegTree
    {
        private readonly int _size;
        private readonly int[] _tree;

        public SegTree(int size)
        {
            _size = size;
            _tree = new int[2 * size];
        }

        public SegTree(IReadOnlyList<int> data) : this(data.Count)
        {
            BuildTree(data);
        }

        private void BuildTree(IReadOnlyList<int> data)
        {
            // 0 0 0 0 1 2 3 4
            for (int i = _size; i < _tree.Length; i++)
            {
                _tree[i] = data[i - _size];
            }

            // 0    0    0    0    1 2 3 4
            // 0    0    0    3+4  1 2 3 4
            // 0    0    1+2  7    1 2 3 4
            // 0    3+7  3    7    1 2 3 4
            // 0    10   3    7    1 2 3 4
            for (int i = _size - 1; i > 0; i--)
            {
                _tree[i] =
                    _tree[2 * i]      // Левый ребенок
                    +                   // Дерево суммы отрезков, поэтому "+"
                    _tree[2 * i + 1]; // Правый ребенок
            }
        }

        public int GetSum(int left, int right)
        {
            var result = 0;

            var lPointer = left + _tree.Length;
            var rPointer = right + _tree.Length;

            for (; lPointer <= rPointer; lPointer /= 2, rPointer /= 2)
            {
                // Если левый указатель указывает на правого потомка
                if (lPointer % 2 == 1)
                    result += _tree[lPointer++];

                // Если правый указатель указывает на левого потомка
                if (rPointer % 2 == 0)
                    result += _tree[rPointer--];
            }

            return result;
        }

        public void Update(int index, int value)
        {
            // 2 5
            // 0 10 3 7 1 2 3 4

            var treeIndex = index + _size; // 2 + 4

            _tree[treeIndex] += value; // 3 + 5

            // 0 10 3 7   1 2 8 4
            // 0 10 3 12  1 2 8 4
            // 0 15 3 12  1 2 8 4
            for (int i = treeIndex / 2; i > 0; i /= 2)
            {
                _tree[i] = _tree[2 * i] + _tree[2 * i + 1];
            }
        }
    }
}