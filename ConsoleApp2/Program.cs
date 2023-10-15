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

        var nums = new List<int>() {
                                                                30,70,

                            8, 25,                              40, 50,                                 76, 88,

            1, 3, 7,    15, 21, 23,     26, 28,     35, 38,     42, 49,     56, 67,     71, 73, 75,     77, 85,     89, 97};

        var bTree = new BTree(3);

        nums.ForEach(bTree.Add);

        var bTree2 = new BTree(2);

        Enumerable.Range(1, 10).ToList().ForEach(bTree2.Add);


        Console.ReadKey();
    }
}