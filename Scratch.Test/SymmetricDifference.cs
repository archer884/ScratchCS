using System.Collections.Generic;
using System.Linq;
using Xunit;

using SystemUnderTest = Scratch.SymmetricDifference;

namespace Scratch.Test
{
    public class SymmetricDifference
    {
        [Fact]
        public void External_difference_works()
        {
            var left = Lhs();
            var right = Rhs();

            var leftResults = new List<int>();
            var rightResults = new List<int>();

            foreach (var taggedValue in left.Difference(right))
            {
                switch (taggedValue)
                {
                    case SystemUnderTest.Left<int> tagged:
                        leftResults.Add(tagged.Item);
                        break;

                    case SystemUnderTest.Right<int> tagged:
                        rightResults.Add(tagged.Item);
                        break;
                }
            }

            var leftConclusion = Enumerable.Range(1, 1000)
                .Where(x => x % 23 == 0 && x % 13 != 0)
                .All(x => leftResults.Contains(x));

            var rightConclusion = Enumerable.Range(1, 1000)
                .Where(x => x % 13 == 0 && x % 23 != 0)
                .All(x => rightResults.Contains(x));

            Assert.True(leftConclusion);
            Assert.True(rightConclusion);
        }

        [Fact]
        public void Internal_difference_works()
        {
            var left = Lhs();
            var right = Rhs();

            var leftResults = new List<int>();
            var rightResults = new List<int>();

            left.IterDifference(right, x =>
            {
                switch (x)
                {
                    case SystemUnderTest.Left<int> tagged:
                        leftResults.Add(tagged.Item);
                        break;

                    case SystemUnderTest.Right<int> tagged:
                        rightResults.Add(tagged.Item);
                        break;
                }
            });

            var leftConclusion = Enumerable.Range(1, 1000)
                .Where(x => x % 23 == 0 && x % 13 != 0)
                .All(x => leftResults.Contains(x));

            var rightConclusion = Enumerable.Range(1, 1000)
                .Where(x => x % 13 == 0 && x % 23 != 0)
                .All(x => rightResults.Contains(x));

            Assert.True(leftConclusion);
            Assert.True(rightConclusion);
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
