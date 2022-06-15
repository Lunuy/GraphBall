#nullable enable
using System;

namespace Assets.Scripts.ExprEval
{
    internal class EvaluableAst
    {
        private int _astId;

        public EvaluableAst(int astId) => _astId = astId;

        ~EvaluableAst()
        {
            if (_astId == 0) return;
            Epp.DisposeAst(_astId);
            _astId = 0;
        }

        public double Eval((string, double)[] variables)
        {
            if (_astId == 0)
            {
                throw new InvalidOperationException("EvaluableAst not initialized");
            }

            return Epp.EvalAst(_astId, variables);
        }
    }
}
