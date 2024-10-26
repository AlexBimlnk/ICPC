using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2024.SegTreeTraining;

/// <summary>
/// <see href="https://codeforces.com/problemset/problem/1234/D"/>
/// </summary>
/// <remarks>
/// Submit
/// </remarks>
public static class D1234
{
    public static void Run()
    {
        var rawString = Console.ReadLine()!;

        var tree = new HashSet<char>[2 * rawString.Length];

        for (int i = rawString.Length; i < tree.Length; i++)
        {
            tree[i] = new HashSet<char>() { rawString[i - rawString.Length] };
        }

        for (int i = rawString.Length - 1; i > 0; i--)
        {
            tree[i] = new HashSet<char>();
            tree[i].UnionWith(tree[2 * i]);
            tree[i].UnionWith(tree[2 * i + 1]);
        }

        var count = int.Parse(Console.ReadLine()!);

        for (int i = 0; i < count; i++)
        {
            var input = Console.ReadLine()!.Split(' ');

            if (input[0] == "1")
            {
                var index = int.Parse(input[1]) - 1;
                var letter = input[2][0];


                var treeIndex = rawString.Length + index;

                var oldLetter = tree[treeIndex].Single();

                tree[treeIndex].Remove(oldLetter);
                tree[treeIndex].Add(letter);

                for (int j = treeIndex / 2; j > 0; j /= 2)
                {
                    tree[j].Clear();
                    tree[j].UnionWith(tree[2 * j]);
                    tree[j].UnionWith(tree[2 * j + 1]);
                }
            }
            else
            {
                var leftIndex = int.Parse(input[1]) - 1;
                var rightIndex = int.Parse(input[2]) - 1;

                var leftTreeIndex = leftIndex + rawString.Length;
                var rightTreeIndex = rightIndex + rawString.Length;

                var result = new HashSet<char>();

                for (; leftTreeIndex <= rightTreeIndex; leftTreeIndex /= 2, rightTreeIndex /= 2)
                {
                    // Если левый указатель указывает на правого потомка
                    if (leftTreeIndex % 2 == 1)
                        result.UnionWith(tree[leftTreeIndex++]);

                    // Если правый указатель указывает на левого потомка
                    if (rightTreeIndex % 2 == 0)
                        result.UnionWith(tree[rightTreeIndex--]);
                }

                Console.WriteLine(result.Count);
            }
        }
    }
}
