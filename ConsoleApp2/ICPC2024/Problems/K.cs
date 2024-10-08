using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.ICPC2024.Problems;

public static class K
{
    private static Dictionary<long, long> memory = new Dictionary<long, long>()
    {
        { 1, 1 },
        { 2, 1 },
        { 3, 2 },
        { 4, 7 },
        { 5, 14 },
    };

    private static long mod = 1000_000_000 + 7;

    public static void Run()
    {
        // Ai = A(i-1) + 2 * A(i-2) + 3 * A(i-3) % mod

        var n = long.Parse(Console.ReadLine());

        Console.WriteLine(Get(n));
    }

    public static long Get(long n)
    {
        if (n < 0) 
            return 0;

        if (memory.TryGetValue(n, out var result)) 
            return result;

        //memory[n] = (8 * Get(n - 3) + 17 * Get(n - 4) + 42 * Get(n - 5)) % mod;

        return memory[n];
    }
}
