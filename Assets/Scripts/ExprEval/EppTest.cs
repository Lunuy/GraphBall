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
            var parseResult = ExprParser.Parse("1 + ", new[] {"x"});
            Debug.Log(parseResult);
        }
    }
}
