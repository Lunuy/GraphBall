#nullable enable
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public struct GraphSampler<T>
    {
        public GraphSampler(double minX, double maxX, double step, Func<T, double, double> function)
        {
            MinX = minX;
            MaxX = maxX;
            Step = step;
            Function = function;
        }

        public double MinX;
        public double MaxX;
        public double Step;
        public Func<T, double, double> Function;

        public void Sample(T t, List<double> result)
        {
            result.Clear();
            var length = (int) Math.Floor((MaxX - MinX) / Step);

            var x = MinX;
            for (var i = 0; i < length; i++)
            {
                result.Add(Function(t, x));
                x += Step;
            }
        }
    }
}
