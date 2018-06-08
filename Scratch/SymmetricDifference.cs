using System;
using System.Collections.Generic;

namespace Scratch
{
    public static class SymmetricDifference
    {
        public static class Tag
        {
            public static Left<T> Left<T>(T item)
            {
                return new Left<T> { Item = item };
            }

            public static Right<T> Right<T>(T item)
            {
                return new Right<T> { Item = item };
            }
        }

        public abstract class Tag<T>
        {
            public T Item { get; set; }
        }

        public class Left<T> : Tag<T>
        {
        }

        public class Right<T> : Tag<T>
        {
        }

        public static IEnumerable<Tag<T>> Difference<T>(this IEnumerable<T> collection, IEnumerable<T> rhs)
            where T : IComparable<T>
        {
            var iterLeft = collection.GetEnumerator();
            var iterRight = rhs.GetEnumerator();

            var hasLeft = iterLeft.MoveNext();
            var hasRight = iterRight.MoveNext();

            while (hasLeft || hasRight)
            {
                var left = iterLeft.Current;
                var right = iterRight.Current;

                if (!hasLeft && !hasRight)
                {
                    yield break;
                }

                if (hasLeft && !hasRight)
                {
                    yield return Tag.Left(left);
                    while (iterLeft.MoveNext())
                    {
                        yield return Tag.Left(iterLeft.Current);
                    }
                    yield break;
                }

                if (hasRight && !hasLeft)
                {
                    yield return Tag.Right(right);
                    while (iterRight.MoveNext())
                    {
                        yield return Tag.Right(iterRight.Current);
                    }
                    yield break;
                }

                var comparison = left.CompareTo(right);
                if (comparison < 0)
                {
                    yield return Tag.Left(left);
                    hasRight = iterLeft.MoveNext();
                }
                else if (comparison > 0)
                {
                    yield return Tag.Right(right);
                    hasRight = iterRight.MoveNext();
                }
                else
                {
                    hasLeft = iterLeft.MoveNext();
                    hasRight = iterRight.MoveNext();
                }
            }
        }

        public static void IterDifference<T>(this IEnumerable<T> collection, IEnumerable<T> rhs, Action<Tag<T>> action)
            where T: IComparable<T>
        {
            var iterLeft = collection.GetEnumerator();
            var iterRight = rhs.GetEnumerator();

            var hasLeft = iterLeft.MoveNext();
            var hasRight = iterRight.MoveNext();

            while (hasLeft || hasRight)
            {
                var left = iterLeft.Current;
                var right = iterRight.Current;

                if (!hasLeft && !hasRight)
                {
                    return;
                }

                if (hasLeft && !hasRight)
                {
                    action(Tag.Left(left));
                    while (iterLeft.MoveNext())
                    {
                        action(Tag.Left(iterLeft.Current));
                    }
                    return;
                }

                if (hasRight && !hasLeft)
                {
                    action(Tag.Right(right));
                    while (iterRight.MoveNext())
                    {
                        action(Tag.Right(iterRight.Current));
                    }
                    return;
                }

                var comparison = left.CompareTo(right);
                if (comparison < 0)
                {
                    action(Tag.Left(left));
                    hasLeft = iterLeft.MoveNext();
                }
                else if (comparison > 0)
                {
                    action(Tag.Right(right));
                    hasRight = iterRight.MoveNext();
                }
                else
                {
                    hasLeft = iterLeft.MoveNext();
                    hasRight = iterRight.MoveNext();
                }
            }
        }
    }
}
