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
        public GameObject? LinePrefab;

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
            if(LinePrefab == null) return;

            // Create axis line
            _xAxisGameObject = Instantiate(LinePrefab);
            _yAxisGameObject = Instantiate(LinePrefab);
            _xAxisGameObject.transform.SetParent(this.transform);
            _yAxisGameObject.transform.SetParent(this.transform);
            _xAxisGameObject.transform.localPosition = new Vector3(0, 0, 1);
            _yAxisGameObject.transform.localPosition = new Vector3(0, 0, 1);
            _xAxisGameObject.transform.localScale = Vector2.one;
            _yAxisGameObject.transform.localScale = Vector2.one;

            _xAxisLineRenderer = _xAxisGameObject.GetComponent<LineRenderer>();
            _yAxisLineRenderer = _yAxisGameObject.GetComponent<LineRenderer>();
            _xAxisLineRenderer.useWorldSpace = false;
            _yAxisLineRenderer.useWorldSpace = false;


            RenderAxis();
        }

        void Update() {
            // _xAxisGameObject!.transform.position = this.transform.position;
            // _yAxisGameObject!.transform.position = this.transform.position;
            // _xAxisGameObject.transform.rotation = this.transform.rotation;
            // _yAxisGameObject.transform.rotation = this.transform.rotation;
            // _xAxisGameObject.transform.localScale = this.transform.localScale;
            // _yAxisGameObject.transform.localScale = this.transform.localScale;
        }

        void RenderAxis() {
            if(_xAxisLineRenderer == null) return;
            if(_yAxisLineRenderer == null) return;

            // Set style
            _xAxisLineRenderer.startWidth = _xAxisLineRenderer.endWidth = 0.1f;
            _yAxisLineRenderer.startWidth = _yAxisLineRenderer.endWidth = 0.1f;


            // X Axis Line
            var xAxisPoints = new NativeArray<Vector3>(
                2,
                Allocator.Temp,
                NativeArrayOptions.UninitializedMemory
            );

            xAxisPoints[0] = new Vector2(0, _options.Origin.y) * _options.Unit;
            xAxisPoints[1] = new Vector2(_options.Size.x, _options.Origin.y) * _options.Unit;

            _xAxisLineRenderer.SetPositions(xAxisPoints);
            _xAxisLineRenderer.positionCount = _options.Origin.y > _options.Size.y || _options.Origin.y < 0 ? 0 : xAxisPoints.Length;


            // Y Axis Line
            var yAxisPoints = new NativeArray<Vector3>(
                2,
                Allocator.Temp,
                NativeArrayOptions.UninitializedMemory
            );

            yAxisPoints[0] = new Vector2(_options.Origin.x, 0) * _options.Unit;
            yAxisPoints[1] = new Vector2(_options.Origin.x, _options.Size.y) * _options.Unit;

            _yAxisLineRenderer.SetPositions(yAxisPoints);
            _yAxisLineRenderer.positionCount = _options.Origin.x > _options.Size.x || _options.Origin.x < 0 ? 0 : xAxisPoints.Length;
        }
    }
}