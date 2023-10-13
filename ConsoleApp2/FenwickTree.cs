using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;
internal class FenwickTree
{
    private abstract class BIT
    {
        protected readonly int[] _tree;

        protected BIT(IReadOnlyList<int> data)
        {
            _tree = new int[data.Count];
            Build(data);
        }

        protected BIT(IReadOnlyList<int> data, int initialValue)
        {
            _tree = new int[data.Count];
            Array.Fill(_tree, initialValue);
            Build(data);
        }

        protected int GetPreviousBucketIndex(int current)
        {
            return (current & (current + 1)) - 1;
        }

        protected int GetNextBucketIndex(int current)
        {
            return current | (current + 1);
        }

        protected abstract void Build(IReadOnlyList<int> data);
    }

    private class BITSum : BIT
    {
        public BITSum(IReadOnlyList<int> data) : base(data) { }

        protected override void Build(IReadOnlyList<int> data)
        {
            for (int i = 0; i < _tree.Length; i++)
            {
                Increment(i, data[i]);
            }
        }

        public void Increment(int index, int value)
        {
            for (int i = index; i < _tree.Length; i = GetNextBucketIndex(i))
            {
                _tree[i] += value;
            }
        }

        private int GetSum(int index)
        {
            var result = 0;

            for (int i = index; i > 0; i = GetPreviousBucketIndex(i))
            {
                result += _tree[i];
            }

            return result;
        }

        public int GetSum(int left, int right)
        {
            return GetSum(right) - GetSum(left - 1);
        }
    }

    private class BITMax : BIT
    {
        public BITMax(IReadOnlyList<int> data) : base(data, int.MinValue) { }

        protected override void Build(IReadOnlyList<int> data)
        {
            for (int i = 0; i < _tree.Length; i++)
            {
                UpdateBuckets(i, data[i]);
            }
        }

        public void UpdateBuckets(int index, int value)
        {
            for (int i = index; i < _tree.Length; i = GetNextBucketIndex(i))
            {
                _tree[i] = Math.Max(_tree[i], value);
            }
        }

        public int GetMax(int index)
        {
            var result = int.MinValue;

            for (int i = index; i > 0; i = GetPreviousBucketIndex(i))
            {
                result = Math.Max(_tree[i], result);
            }

            return result;
        }
    }

    public static void Start()
    {
        var input = Console.ReadLine()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        var bit = new BITMax(input);

        Console.WriteLine(bit.GetMax(5));
    }
}
