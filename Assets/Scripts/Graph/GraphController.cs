#nullable enable
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts.Graph
{
    [RequireComponent(typeof(GraphSamplerComponent), typeof(GraphRenderer), typeof(GraphCollider)), RequireComponent(typeof(Gridder), typeof(AxisRenderer))]
    public class GraphController : MonoBehaviour
    {
        private double _minX;

        public double MinX
        {
            get => _graphSampler == null ? _minX : _graphSampler.Options.MinX;
            set
            {
                _minX = value;

                if (_graphSampler == null) return;
                if (_gridder == null) return;
                if (_axisRenderer == null) return;
                
                _graphSampler.Options = new GraphSamplerComponentOptions { 
                    MinX = value,
                    MaxX = _graphSampler.Options.MaxX,
                    Step = _graphSampler.Options.Step
                };
                _gridder.Options = new GridderOptions {
                    MinX = value,
                    MaxX = _gridder.Options.MaxX,
                    MinY = _gridder.Options.MinY
                };
            }
        }

        private double _maxX = 10;

        public double MaxX {
            get => _graphSampler == null ? _maxX : _graphSampler.Options.MaxX;
            set
            {
                _maxX = value;

                if (_graphSampler == null) return;
                if (_gridder == null) return;
                if (_axisRenderer == null) return;
                
                _graphSampler.Options = new GraphSamplerComponentOptions { 
                    MinX = _graphSampler.Options.MinX,
                    MaxX = value,
                    Step = _graphSampler.Options.Step
                };
                _gridder.Options = new GridderOptions {
                    MinX = _gridder.Options.MinX,
                    MaxX = value,
                    MinY = _gridder.Options.MinY
                };
            }
        }

        private double _step = 0.05;

        public double Step {
            get => _graphSampler == null ? _step : _graphSampler.Options.Step;
            set
            {
                _step = value;

                if (_graphSampler == null) return;
                if (_gridder == null) return;
                
                _graphSampler.Options = new GraphSamplerComponentOptions { 
                    MinX = _graphSampler.Options.MinX,
                    MaxX = _graphSampler.Options.MaxX,
                    Step = value
                };
            }
        }

        private double _minY;

        public double MinY {
            get => _minY;
            set {
                _minY = value;

                if (_gridder == null) return;
                _gridder.Options = new GridderOptions {
                    MinX = _gridder.Options.MinX,
                    MaxX = _gridder.Options.MaxX,
                    MinY = value
                };
            }
        }
        public double MaxY => _minY + (MaxX - MinX) * (transform.localScale.y / transform.localScale.x);

        private GraphSamplerComponent? _graphSampler;
        private GraphRenderer? _graphRenderer;
        private GraphCollider? _graphCollider;
        private Gridder? _gridder;
        private AxisRenderer? _axisRenderer;

        // ReSharper disable once UnusedMember.Global
        public void Start()
        {
            _graphSampler = GetComponent<GraphSamplerComponent>();
            _graphSampler.Options = new GraphSamplerComponentOptions()
            {
                MaxX = _maxX,
                MinX = _minX,
                Step = _step
            };

            _graphRenderer = GetComponent<GraphRenderer>();
            _graphCollider = GetComponent<GraphCollider>();
            _gridder = GetComponent<Gridder>();
            _axisRenderer = GetComponent<AxisRenderer>();

            _graphSampler.OnSample += OnSample;
        }
        
        // ReSharper disable once UnusedMember.Global
        public void Update() {
            var sampleUnit = new Vector2(1.0f / _graphSampler!.SampledYList.Count, 1.0f / (float)(MaxY - _minY));

            _graphRenderer!.Options = new GraphRendererOptions { Unit = sampleUnit, Offset = new Vector2(0, (float)-_minY) };
            _graphCollider!.Options = new GraphColliderOptions { Unit = sampleUnit, Offset = new Vector2(0, (float)-_minY) };

            var unit = new Vector2(1.0f / (float)(_maxX - _minX), 1.0f / (float)(MaxY - _minY));
            _axisRenderer!.Options = new AxisRendererOptions {
                Origin = new Vector2((float)_minX, (float)-_minY),
                Size = new Vector2((float)(_maxX - _minX), (float)(MaxY - _minY)),
                Unit = unit
            };
        }

        private readonly List<double> _validatedYArray = new();

        private void OnSample(IReadOnlyList<double> yArray)
        {
            var maxY = _minY + (MaxX - MinX) * (transform.localScale.y / transform.localScale.x);

            _validatedYArray.Clear();
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < yArray.Count; i++)
            {
                _validatedYArray.Add(Math.Min(Math.Max(yArray[i], _minY), maxY));
            }

            _graphRenderer!.YArray = _validatedYArray;
            _graphCollider!.YArray = _validatedYArray;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            _graphSampler!.OnSample -= OnSample;
        }
    }
}
