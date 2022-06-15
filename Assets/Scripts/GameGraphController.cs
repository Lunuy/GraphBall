#nullable enable
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(GraphSamplerComponent), typeof(GraphRenderer), typeof(GraphCollider))]
    public class GameGraphController : MonoBehaviour
    {
        public double Width = 10;
        public double Height = 10;
        public double MinX = 0;
        public double MaxX = 10;
        public double MinY = 0;
        public double MaxY = 10;
        public double Step = 0.1;

        private GraphSamplerComponent? _graphSampler;
        private GraphRenderer? _graphRenderer;
        private GraphCollider? _graphCollider;
        
        // ReSharper disable once UnusedMember.Global
        public void Start()
        {
            _graphSampler = GetComponent<GraphSamplerComponent>();
            _graphRenderer = GetComponent<GraphRenderer>();
            _graphCollider = GetComponent<GraphCollider>();

            _graphSampler.OnSample += OnSample;

            var unit = Width/(MaxX - MinX);
            _graphSampler.Options = new GraphSamplerComponentOptions { MinX = MinX, MaxX = MaxX, Step = Step };
            _graphRenderer.Options = new GraphRendererOptions { Unit = unit, Step = Step };
            _graphCollider.Options = new GraphColliderOptions { Unit = unit, Step = Step };
        }

        private void OnSample(IReadOnlyList<double> yArray)
        {
            _graphRenderer!.YArray = yArray;
            _graphRenderer.Render();
            _graphCollider!.YArray = yArray;
            _graphCollider.UpdateCollider();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            _graphSampler!.OnSample -= OnSample;
        }
    }
}
