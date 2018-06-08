using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using static Scratch.SymmetricDifference;

namespace Scratch
{
    class Program
    {
        static void Main(string[] args)
        {
            var left = Lhs();
            var right = Rhs();

            Time(() =>
            {
                var sum = left.Difference(right)
                    .Where(tagged => tagged is Left<int>)
                    .Sum(tagged => tagged.Item);

                Console.WriteLine(sum);
            });

            Time(() =>
            {
                var sum = 0;
                left.IterDifference(right, tagged =>
                {
                    if (tagged is Left<int> x)
                    {
                        sum += x.Item;
                    }
                });

                Console.WriteLine(sum);
            });

            Time(() =>
            {
                var leftSet = new HashSet<int>(left);
                var rightSet = new HashSet<int>(right);

                leftSet.ExceptWith(rightSet);
                Console.WriteLine(leftSet.Sum());
            });
        }

        static void Time(Action action)
        {
            var time = Stopwatch.StartNew();
            action();
            time.Stop();
            Console.WriteLine(time.Elapsed.TotalMilliseconds);
        }

        static List<int> Lhs()
        {
            return Enumerable.Range(1, 1000).Where(x => x % 13 != 0).ToList();
        }

        static List<int> Rhs()
        {
            return Enumerable.Range(0, 1000).Where(x => x % 23 != 0).ToList();
        }
    }
}
