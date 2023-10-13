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

    public static void Start()
    {
        var input = Console.ReadLine()
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        var bit = new BITSum(input);

        Console.WriteLine(bit.GetSum(3, 5));
    }
}
