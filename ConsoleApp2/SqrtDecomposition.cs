using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2;

/// <summary>
/// Корневая декомпозиция.
/// </summary>
public sealed class SqrtDecomposition
{
    /// <summary>
    /// Пример. Найти сумму на отрезке.
    /// </summary>
    public static void SumOnSegment()
    {
        var nums = Console.ReadLine()!
            .Split(' ')
            .Select(int.Parse) 
            .ToList();

        // Топорное решение будет медленным при большом кол-ве запросов
        int Native()
        {
            var left = 1;
            var right = 2;
            var result = 0;
            for (int i = left; i <= right; i++)
            {
                result += nums[i];
            }

            return result;
        }

        int Optimization()
        {
            //nums: 1 2 3 4

            //prefixSum: 0 1 3 6 10
            var prefixSum = new int[nums.Count + 1];

            // Заполняем префиксными суммами
            for (int i = 0; i < nums.Count; i++)
            {
                prefixSum[i + 1] = prefixSum[i] + nums[i];
            }

            var left = 1;
            var right = 2;
            
            // expected: 5

            return prefixSum[right + 1] - prefixSum[left];
        }

        Console.WriteLine(Optimization());
    }

    /// <summary>
    /// Пример. Найти сумму на отрезке. Число Ai в массиве данных может быть изменено.
    /// </summary>
    public static void SumOnSegmentWithChangeArray()
    {
        var nums = Console.ReadLine()!
            .Split(' ')
            .Select(int.Parse)
            .ToList();

        // При изменении придется пересчитывать все префиксы, так что
        // бьем массив на отрезки sqrt(n) со своими посчитаными суммами
        // как итог считает блоки + элементы -> счет чуть медленнее
        // но при вставке работает без пересчетов сегментов за О(1)
        int Solution()
        {
            //nums: 1 2 3 4

            var segmentSize = (int)Math.Sqrt(nums.Count);

            var segmentCount = nums.Count / segmentSize + 1;

            var segmentSums = new int[segmentCount];

            int GetSegmentIndexByGlobalIndex(int index)
            {
                return index / segmentSize; // Благодаря отбрасыванию дробной части получаем индекс бакета
            }

            void Init()
            {
                for (int i = 0; i < nums.Count; ++i)
                {
                    segmentSums[GetSegmentIndexByGlobalIndex(i)] += nums[i]; 
                }
            }

            void Update(int index, int value)
            {
                nums[index] += value; 
            }

            int GetSum(int left, int rigth)
            {
                var result = 0;

                // Если диапазон в одном сегменте просто счет
                if (GetSegmentIndexByGlobalIndex(left) == GetSegmentIndexByGlobalIndex(rigth))
                {
                    for (int i = left; i <= rigth; ++i)
                    {
                        result += nums[i];
                    }
                }
                else
                {
                    // От левой границы диапазона до конца бакета
                    for (
                        int i = left; 
                        GetSegmentIndexByGlobalIndex(i) == GetSegmentIndexByGlobalIndex(left);
                        i++)
                    {
                        result += nums[i];
                    }

                    // Промежуточные суммы бакетов просто складываем
                    for (
                        int i = GetSegmentIndexByGlobalIndex(left) + 1;
                        i < GetSegmentIndexByGlobalIndex(rigth);
                        i++)
                    {
                        result += segmentSums[i];
                    }

                    // От правой границы диапазона до начала бакета
                    for (
                        int i = rigth;
                        GetSegmentIndexByGlobalIndex(i) == GetSegmentIndexByGlobalIndex(rigth);
                        i--)
                    {
                        result += nums[i];
                    }
                }

                return result;
            }

            var left = 1;
            var right = 2;

            return GetSum(left, right);
        }

        Console.WriteLine(Solution());
    }
}
