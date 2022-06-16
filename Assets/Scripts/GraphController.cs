#nullable enable
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    [RequireComponent(typeof(GraphSamplerComponent), typeof(GraphRenderer), typeof(GraphCollider)), RequireComponent(typeof(Gridder))]
    public class GraphController : MonoBehaviour
    {
        public double MinX {
            get => _graphSampler == null ? 0 : _graphSampler.Options.MinX;
            set {
                if (_graphSampler == null) return;
                if (_gridder == null) return;
                
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
        public double MaxX {
            get => _graphSampler == null ? 0 : _graphSampler.Options.MaxX;
            set {
                if (_graphSampler == null) return;
                if (_gridder == null) return;
                
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

        public double Step {
            get => _graphSampler == null ? 0 : _graphSampler.Options.Step;
            set {
                if (_graphSampler == null) return;
                if (_gridder == null) return;
                
                _graphSampler.Options = new GraphSamplerComponentOptions { 
                    MinX = _graphSampler.Options.MinX,
                    MaxX = _graphSampler.Options.MaxX,
                    Step = value
                };
            }
        }

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
        
        private double _minY;

        // ReSharper disable once UnusedMember.Global
        public void Start()
        {
            _graphSampler = GetComponent<GraphSamplerComponent>();
            _graphRenderer = GetComponent<GraphRenderer>();
            _graphCollider = GetComponent<GraphCollider>();
            _gridder = GetComponent<Gridder>();

            _graphSampler.OnSample += OnSample;
        }
        
        // ReSharper disable once UnusedMember.Global
        public void Update() {
            var unit = new Vector2(1.0f / _graphSampler!.SampledYList.Count, 1.0f / (float)(MaxY - _minY));

            _graphRenderer!.Options = new GraphRendererOptions { Unit = unit, Offset = new Vector2(0, (float)_minY) };
            _graphCollider!.Options = new GraphColliderOptions { Unit = unit, Offset = new Vector2(0, (float)_minY) };
        }

        private void OnSample(IReadOnlyList<double> yArray)
        {
            double maxY = _minY + (MaxX - MinX) * (transform.localScale.y / transform.localScale.x);

            double[] validatedYArray = new double[yArray.Count];
            for (int i = 0; i < yArray.Count; i++)
            {
                validatedYArray[i] = Math.Min(Math.Max(yArray[i], _minY), maxY);
            }

            _graphRenderer!.YArray = Array.AsReadOnly(validatedYArray);
            _graphCollider!.YArray = Array.AsReadOnly(validatedYArray);
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            _graphSampler!.OnSample -= OnSample;
        }
    }
}
