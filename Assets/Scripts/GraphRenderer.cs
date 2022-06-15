#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public struct GraphRendererOptions
    {
        public double Unit;
        public double Step;
    }

    [RequireComponent(typeof(LineRenderer))]
    public class GraphRenderer : MonoBehaviour
    {
        private LineRenderer? _lineRenderer;

        public GraphRendererOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                Render();
            }
        }

        public IReadOnlyList<double> YArray
        {
            get => _yArray;
            set
            {
                _yArray = value;
                Render();
            }
        }

        private GraphRendererOptions _options;
        private IReadOnlyList<double> _yArray = Array.AsReadOnly(new double[] { });

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>()!;
            Render();
        }

        public void Render()
        {
            if (_lineRenderer == null) return;

            var points = new Vector3[_yArray.Count];

            for (var i = 0; i < _yArray.Count; i++)
            {
                points[i] = new Vector3(
                    (float) (i * _options.Step * _options.Unit),
                    (float) (_yArray[i] * _options.Unit),
                    0
                );
            }

            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
        }
    }
}
