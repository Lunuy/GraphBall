#nullable enable
using System;

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

        public double[] Sample(T t)
        {
            var sampledYArray = new double[(int) Math.Floor((MaxX - MinX) / Step)];

            var x = MinX;
            for (var i = 0; i < sampledYArray.Length; i++)
            {
                sampledYArray[i] = Function(t, x);
                x += Step;
            }

            return sampledYArray;
        }
    }
}
