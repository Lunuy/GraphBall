#nullable enable
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(GraphSamplerComponent), typeof(GameGraphController))]
    public class TestGraphComponent : MonoBehaviour
    {
        private GraphSamplerComponent? _graphSampler;
        
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _graphSampler = GetComponent<GraphSamplerComponent>();
            _graphSampler.Variables = new double[]{0};
            _graphSampler.Function = (t, x) => Math.Sin(t[0]*x - t[0]);
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            _graphSampler!.Variables = new[]{ _graphSampler.Variables[0] + Time.deltaTime * 2 };
        }
    }
}
