#nullable enable
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Assets.Scripts.Graph
{
    public struct GraphRendererOptions
    {
        public Vector2 Unit;
        public Vector2 Offset;
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

            var points = new NativeArray<Vector3>(
                _yArray.Count,
                Allocator.Temp,
                NativeArrayOptions.UninitializedMemory
            );

            for (var i = 0; i < _yArray.Count; i++)
            {
                points[i] = (_options.Offset + new Vector2(i, (float) _yArray[i])) * _options.Unit;
            }

            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
            
            points.Dispose();
        }
    }
}
