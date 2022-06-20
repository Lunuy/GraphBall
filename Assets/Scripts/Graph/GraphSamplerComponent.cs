#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.Scripts.Graph
{
    public struct GraphSamplerComponentOptions
    {
        public double MinX;
        public double MaxX;
        public double Step;
    }

    [RequireComponent(typeof(LineRenderer))]
    public class GraphSamplerComponent : MonoBehaviour
    {
        public delegate void SampleHandler(IReadOnlyList<double> sampledYList);

        public event SampleHandler OnSample = delegate { };

        public GraphSamplerComponentOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                _graphSampler.MinX = value.MinX;
                _graphSampler.MaxX = value.MaxX;
                _graphSampler.Step = value.Step;
                Sample();
            }
        }

        private GraphSamplerComponentOptions _options;

        public double[] Variables
        {
            get
            {
                var variables = new double[_variables.Length];
                Array.Copy(_variables, variables, _variables.Length);
                return variables;
            }
            set
            {
                if (value.Length != _variables.Length)
                {
                    _variables = new double[value.Length];
                }
                Array.Copy(value, _variables, value.Length);
                Sample();
            }
        }

        public double GetVariablesNth(int index) => _variables[index];
        public double GetVariablesLength() => _variables.Length;

        public IReadOnlyList<double> SampledYList => _sampledYList;

        public Func<double[], double, double> Function
        {
            get => _graphSampler.Function;
            set
            {
                _graphSampler.Function = value;
                Sample();
            }
        }

        private Func<double[], double, double> _function = (t, x) => x * x * 0.1;
        private readonly List<double> _sampledYList = new();
        private GraphSampler<double[]> _graphSampler = new(0, 0, 1, (t, x) => x);
        private double[] _variables = { };

        private void Sample()
        {
            _graphSampler.Sample(_variables, _sampledYList);
            OnSample.Invoke(_sampledYList);
        }
    }
}
