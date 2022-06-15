#nullable enable
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Assets.Scripts
{
    public struct GraphSamplerComponentOptions {
        public double MinX;
        public double MaxX;
        public double Step;
    }

    [RequireComponent(typeof(LineRenderer))]
    public class GraphSamplerComponent : MonoBehaviour
    {
        public delegate void SampleHandler(ReadOnlyCollection<double> sampledYList);
        public event SampleHandler OnSample = delegate { };

        public GraphSamplerComponentOptions Options {
            get => _options;
            set {
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
            get => _variables;
            set {
                _variables = value;
                Sample();
            }
        }
    
        public ReadOnlyCollection<double> SampledYList => Array.AsReadOnly(_sampledYList);
        public Func<double[], double, double> Function
        { 
            get => _graphSampler.Function; 
            set {
                _graphSampler.Function = value;
                this.Sample();
            } 
        }
    
        private Func<double[], double, double> _function = (t, x) => x * x * 0.1;
        private double[] _sampledYList = {};
        private GraphSampler<double[]> _graphSampler = new(0, 0, 1, (t, x) => x);
        private double[] _variables = {};

        private void Sample() {
            _sampledYList = _graphSampler.Sample(_variables);
            OnSample.Invoke(SampledYList);
        }
    }
}