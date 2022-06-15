#nullable enable
using UnityEngine;

namespace Assets.Scripts.ExprEval
{
    // ReSharper disable once UnusedMember.Global
    public class EppTest : MonoBehaviour
    {
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            using var parseResult = ExprParser.Parse("1 + x", new[] {"x"});
            Debug.Log(parseResult.EvaluableAst?.Eval(new[] {("x", 2.0)}) ?? null);
        }
    }
}
