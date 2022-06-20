#nullable enable
using UnityEngine;
using Unity.Collections;


namespace Assets.Scripts.Graph
{
    public struct AxisRendererOptions
    {
        public Vector2 Unit;
        public Vector2 Size;
        public Vector2 Origin;
    }

    [ExecuteInEditMode]
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
        public GameObject XAxisGameObject = null!;
        public GameObject YAxisGameObject = null!;
        private LineRenderer? _xAxisLineRenderer;
        private LineRenderer? _yAxisLineRenderer;
        
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            // Create axis line
            XAxisGameObject.transform.SetParent(transform);
            YAxisGameObject.transform.SetParent(transform);
            XAxisGameObject.transform.localPosition = new Vector3(0, 0, 1);
            YAxisGameObject.transform.localPosition = new Vector3(0, 0, 1);
            XAxisGameObject.transform.localScale = Vector2.one;
            YAxisGameObject.transform.localScale = Vector2.one;

            _xAxisLineRenderer = XAxisGameObject.GetComponent<LineRenderer>();
            _yAxisLineRenderer = YAxisGameObject.GetComponent<LineRenderer>();
            _xAxisLineRenderer.useWorldSpace = false;
            _yAxisLineRenderer.useWorldSpace = false;


            RenderAxis();
        }

        // ReSharper disable once UnusedMember.Local
        //private void Update()
        //{
        //    _xAxisGameObject!.transform.position = this.transform.position;
        //    _yAxisGameObject!.transform.position = this.transform.position;
        //    _xAxisGameObject.transform.rotation = this.transform.rotation;
        //    _yAxisGameObject.transform.rotation = this.transform.rotation;
        //    _xAxisGameObject.transform.localScale = this.transform.localScale;
        //    _yAxisGameObject.transform.localScale = this.transform.localScale;
        //}

        private void RenderAxis() {
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