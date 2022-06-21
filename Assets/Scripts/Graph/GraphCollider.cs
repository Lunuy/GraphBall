#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Graph
{
    public struct GraphColliderOptions
    {
        public Vector2 Unit;
        public Vector2 Offset;
    }

    [RequireComponent(typeof(EdgeCollider2D))]
    public class GraphCollider : MonoBehaviour
    {
        public GraphColliderOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                UpdateCollider();
            }
        }

        public IReadOnlyList<double> YArray
        {
            get => _yArray;
            set
            {
                _yArray = value;
                UpdateCollider();
            }
        }

        private GraphColliderOptions _options;
        private EdgeCollider2D? _edgeCollider2D;
        private IReadOnlyList<double> _yArray = Array.AsReadOnly(new double[] { });

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _edgeCollider2D = GetComponent<EdgeCollider2D>();
            UpdateCollider();
        }

        private readonly List<Vector2> _points = new();

        public void UpdateCollider()
        {
            if (_edgeCollider2D == null) return;

            _points.Clear();

            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < _yArray.Count; i++)
            {
                _points.Add((_options.Offset + new Vector2(i, (float) _yArray[i])) * _options.Unit);
            }

            _edgeCollider2D.SetPoints(_points);

        }
    }
}
