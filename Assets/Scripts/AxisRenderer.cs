#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;


namespace Assets.Scripts
{
    public struct AxisRendererOptions
    {
        public Vector2 Unit;
        public Vector2 Size;
        public Vector2 Origin;
    }

    public class AxisRenderer : MonoBehaviour
    {
        public AxisRendererOptions Options {
            get => _options;
            set {
                _options = value;
                RenderAxis();
            }
        }

        private AxisRendererOptions _options;
        private GameObject? _xAxisGameObject;
        private GameObject? _yAxisGameObject;
        private LineRenderer? _xAxisLineRenderer;
        private LineRenderer? _yAxisLineRenderer;

        // Start is called before the first frame update
        void Start()
        {
            _xAxisGameObject = new GameObject("XAxis");
            _yAxisGameObject = new GameObject("YAxis");
            _xAxisGameObject.transform.SetParent(this.transform);
            _yAxisGameObject.transform.SetParent(this.transform);

            _xAxisLineRenderer = _xAxisGameObject.AddComponent<LineRenderer>();
            _yAxisLineRenderer = _yAxisGameObject.AddComponent<LineRenderer>();

            RenderAxis();
        }

        void RenderAxis() {
            if(_xAxisLineRenderer == null) return;
            if(_yAxisLineRenderer == null) return;


            // X Axis Line
            var xAxisPoints = new NativeArray<Vector3>(
                2,
                Allocator.Temp,
                NativeArrayOptions.UninitializedMemory
            );

            xAxisPoints[0] = new Vector2(0, _options.Origin.y) * _options.Unit;
            xAxisPoints[1] = new Vector2(_options.Size.x, _options.Origin.y) * _options.Unit;

            _xAxisLineRenderer.positionCount = xAxisPoints.Length;
            _xAxisLineRenderer.SetPositions(xAxisPoints);


            // Y Axis Line
            var yAxisPoints = new NativeArray<Vector3>(
                2,
                Allocator.Temp,
                NativeArrayOptions.UninitializedMemory
            );

            yAxisPoints[0] = new Vector2(_options.Origin.x, 0) * _options.Unit;
            yAxisPoints[1] = new Vector2(_options.Origin.x, _options.Size.y) * _options.Unit;

            _yAxisLineRenderer.positionCount = yAxisPoints.Length;
            _yAxisLineRenderer.SetPositions(yAxisPoints);
        }
    }
}