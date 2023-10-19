using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;

public sealed class DSU
{
    private int[] _parent;
    private int[] _size; // для оптимизации объединения

    public DSU(int maxValue)
    {
        _parent = new int[maxValue + 1];
        _size = new int[maxValue + 1];
    }

    public void MakeSet(int value)
    {
        _parent[value] = value;
        _size[value] = 1;
    }

    public int FindSetLeader(int value) // Возвращает лидера множества, в котором находится value
    {
        if (value == _parent[value]) 
            return value;

        return FindSetLeader(_parent[value]);
    }

    // Поиск с оптимизацией не просто скачет по узлам,
    // А запоминает найденный последний
    public int FindSetLeaderOptimize(int value) 
    {
        if (value == _parent[value])
            return value;

        return _parent[value] = FindSetLeader(_parent[value]);
    }

    public void UnionSets(int firstValue, int secondValue) // Объединяет множества в которые входят два элемента
    {
        var firstLeader = FindSetLeaderOptimize(firstValue);
        var secondLeader = FindSetLeaderOptimize(secondValue);

        if (firstLeader != secondLeader)
        {
            // _parent[secondLeader] = firstLeader; // Второй теряет лидерство и встает под первого

            // Оптимизации объединения основывается на размерах множеств -
            // выгодно объединять маленькое с большим а не наоборот
            if (_size[firstLeader] < _size[secondLeader])
                (firstLeader, secondLeader) = (secondLeader, firstLeader);
            
            _parent[secondLeader] = firstLeader;
            _size[firstLeader] += _size[secondLeader];
        }
    }

    private static class Example
    {
        public static void Examaple()
        {
            var nums = new List<int> { 7, 9, 13, 20, 5, 2, 3 };

            var dsu = new DSU(20);

            nums.ForEach(dsu.MakeSet);

            dsu.UnionSets(13, 7);
            dsu.UnionSets(7, 9);
            dsu.UnionSets(20, 5);
            dsu.UnionSets(9, 7);
            dsu.UnionSets(2, 3);
            dsu.UnionSets(3, 5);

            // 5 -> 20 -> 2
            dsu.FindSetLeader(5);
            // 5 -> 20 -> 2
            dsu.FindSetLeaderOptimize(5);
            // 5 -> 2
        }
    }
}
