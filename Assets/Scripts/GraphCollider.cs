#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public struct GraphColliderOptions
    {
        public double Unit;
        public double Step;
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

        public void UpdateCollider()
        {
            if (_edgeCollider2D == null) return;

            var points = new List<Vector2>(_yArray.Count);
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < _yArray.Count; i++)
            {
                points.Add(new Vector2(
                    (float) (i * _options.Step * _options.Unit),
                    (float) (_yArray[i] * _options.Unit)
                ));
            }

            // Debug.Log(points);
            // Debug.Log(points.Count);

            _edgeCollider2D.SetPoints(points);

        }
    }
}
