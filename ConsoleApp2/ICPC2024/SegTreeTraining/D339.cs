using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2024.SegTreeTraining;

/// <summary>
/// <see href="https://codeforces.com/problemset/problem/339/D"/>
/// </summary>
/// <remarks>
/// Submit
/// </remarks>
public static class D339
{
    public static void Run()
    {
        var input = Console.ReadLine()!.Split(' ');

        var n = int.Parse(input[0]);
        var m = int.Parse(input[1]);

        var data = Console.ReadLine()!.Split(' ')
            .Select(long.Parse)
            .ToList();

        var tree = new (long, bool)[data.Count * 2];

        for (int i = data.Count; i < tree.Length; i++)
        {
            tree[i] = (data[i - data.Count], true);
        }

        var or = true;

        var current = 0;
        var layerElements = data.Count;

        for (int i = data.Count - 1; i > 0; i--)
        {
            if (or)
            {
                tree[i] =
                    (tree[2 * i].Item1 | tree[2 * i + 1].Item1, true);
            }
            else
            {
                tree[i] =
                    (tree[2 * i].Item1 ^ tree[2 * i + 1].Item1, false);
            }

            current++;

            if (current == layerElements / 2)
            {
                current = 0;
                layerElements /= 2;
                or = !or;
            }
        }

        for (int i = 0; i < m; i++)
        {
            input = Console.ReadLine()!.Split(' ');

            Update(tree, data.Count, index: int.Parse(input[0]) - 1, value: int.Parse(input[1]));

            Console.WriteLine(tree[1].Item1);
        }
    }

    private static void Update((long, bool)[] tree, int size, int index, long value)
    {
        var treeIndex = size + index;

        tree[treeIndex] = (value, tree[treeIndex].Item2);

        for (int i = treeIndex / 2; i > 0; i /= 2)
        {
            if (tree[i].Item2)
            {
                tree[i].Item1 = tree[2 * i].Item1 | tree[2 * i + 1].Item1;
            }
            else
            {
                tree[i].Item1 = tree[2 * i].Item1 ^ tree[2 * i + 1].Item1;
            }
        }
    }
}
