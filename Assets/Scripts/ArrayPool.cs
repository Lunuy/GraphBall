#nullable enable
using System.Collections.Generic;

namespace EppBind
{
    public static class ArrayPool<T>
    {
        private static readonly Dictionary<int, Stack<T[]>> Pool = new();

        public static T[] Rent(int size)
        {
            if (!Pool.TryGetValue(size, out var stack))
            {
                stack = new Stack<T[]>();
                Pool.Add(size, stack);
            }

            if (0 >= stack.Count) return new T[size];
            var array = stack.Pop();
            return array;
        }

        public static void Return(T[] array)
        {
            if (!Pool.TryGetValue(array.Length, out var stack))
            {
                stack = new Stack<T[]>();
                Pool.Add(array.Length, stack);
            }

            stack.Push(array);
        }
    }
}
