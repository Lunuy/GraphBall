#nullable enable

using Assets.Scripts.ExprEval.Epp;

namespace Assets.Scripts.ExprEval
{
    internal struct ParseResult
    {
        /// <summary>
        /// if parse is success this value will be not null than you can evaluate equation
        /// </summary>
        public readonly EvaluableAst? EvaluableAst;
        
        /// <summary>
        /// if parse has error or warning, this array has that info
        /// 
        /// this value must be readonly but performance reason, it's not immutable.
        /// </summary>
        public readonly Diagnostic[] Diagnostics;

        public ParseResult(EvaluableAst? evaluableAst, Diagnostic[] diagnostics)
        {
            EvaluableAst = evaluableAst;
            Diagnostics = diagnostics;
        }
    }

    internal static class ExprParser
    {
        /// <summary>
        /// parse given expression
        /// </summary>
        /// <param name="expr">equation expression</param>
        /// <param name="idNames">use variable names</param>
        /// <returns></returns>
        public static ParseResult Parse(string expr, string[] idNames)
        {
            var internalParseResult = Epp.Epp.CreateAst(expr, idNames);
            return new ParseResult(
                internalParseResult.AstId == -1
                    ? null
                    : new EvaluableAst(internalParseResult.AstId),
                internalParseResult.Diagnostics
            );
        }
    }
}
