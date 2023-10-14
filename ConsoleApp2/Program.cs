using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConsoleApp2;

//var input = Console.ReadLine()
//        .Split(' ')
//        .Select(int.Parse)
//        .ToList();

internal class Program
{
    

    private static void Main(string[] args)
    {
        //SqrtDecomposition.SumOnSegment();

        //E_Problem_SegTree_Training.Start();

        //FenwickTree.Start();

        var nums = new List<int>() { 12, 34, 7, 1, 8, 9 };

        var bst = new BinarySearchTree();

        nums.ForEach(bst.Add);

        Console.WriteLine(bst.Find(9));
        Console.WriteLine(bst.Find(34));

        bst.Remove(9);

        Console.ReadKey();
    }
}