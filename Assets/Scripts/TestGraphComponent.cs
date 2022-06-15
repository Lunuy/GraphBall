#nullable enable
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(GraphSamplerComponent))]
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
            var variables = ArrayPool<double>.Rent(1);
            variables[0] = _graphSampler!.GetVariablesNth(0) + Time.deltaTime * 2;
            _graphSampler.Variables = variables;
            ArrayPool<double>.Return(variables);
        }
    }
}
