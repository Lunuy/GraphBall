#nullable enable
using System;

namespace Assets.Scripts.ExprEval
{
    /// <summary>
    /// native ast instance wrapper class
    /// </summary>
    internal class EvaluableAst
    {
        private int _astId;

        public EvaluableAst(int astId) => _astId = astId;

        ~EvaluableAst()
        {
            if (_astId == 0) return;
            Epp.Epp.DisposeAst(_astId);
            _astId = 0;
        }

        /// <summary>
        /// evaluate expression with given variables map
        /// </summary>
        /// <param name="variables">
        /// variables map,
        /// this parameter type is not System.Collection.Generic.Dictionary for performance reason but you MUST take unique key array
        /// if variables map is not match with it's equation this method will be panic!
        /// </param>
        /// <returns>evaluated value</returns>
        /// <exception cref="InvalidOperationException">if ast is not initialized, it will throw InvalidOperationException</exception>
        public double Eval((string, double)[] variables)
        {
            if (_astId == 0)
            {
                throw new InvalidOperationException("EvaluableAst not initialized");
            }

            return Epp.Epp.EvalAst(_astId, variables);
        }
    }
}
