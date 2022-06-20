#nullable enable
using Assets.Scripts.ExprEval;
using Assets.Scripts.Utility;
using UnityEngine;
using Assets.Scripts.Graph;

namespace Assets.Scripts
{
    [RequireComponent(typeof(GraphSamplerComponent))]
    public class TestGraphComponent : MonoBehaviour
    {
        public string Expr = "sin(t * x - t)";

        private GraphSamplerComponent? _graphSampler;
        
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            var parseResult = ExprParser.Parse(Expr, new[] { "x", "t" });

            _graphSampler = GetComponent<GraphSamplerComponent>();
            _graphSampler.Variables = new (string, double)[]{("t", 0)};
            _graphSampler.Function = (t, x) =>
            {
                var variables = ArrayPool<(string, double)>.Rent(2);
                variables[0] = ("x", x);
                variables[1] = ("t", t[0].Item2);
                var result = parseResult.EvaluableAst!.Eval(variables);
                ArrayPool<(string, double)>.Return(variables);
                return result;
            };
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            var variables = ArrayPool<(string, double)>.Rent(1);
            variables[0] = ("t", _graphSampler!.GetVariablesNth(0).Item2 + Time.deltaTime * 2);
            _graphSampler.Variables = variables;
            ArrayPool<(string, double)>.Return(variables);
        }
    }
}
