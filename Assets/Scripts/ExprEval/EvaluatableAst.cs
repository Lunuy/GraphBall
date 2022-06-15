#nullable enable
using System;

namespace Assets.Scripts.ExprEval
{
    internal struct EvaluableAst : IDisposable
    {
        private int _astId;

        public EvaluableAst(int astId) => _astId = astId;

        public double Eval((string, double)[] variables)
        {
            if (_astId == 0)
            {
                throw new InvalidOperationException("EvaluableAst not initialized");
            }

            return Epp.EvalAst(_astId, variables);
        }

        public void Dispose()
        {
            if (_astId == 0) return;
            Epp.DisposeAst(_astId);
            _astId = 0;
        }
    }
}
