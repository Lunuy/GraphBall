#nullable enable
using System;
using UnityEngine;

namespace Assets.Scripts.ExprEval
{
    internal struct ParseResult
    {
        public readonly EvaluableAst? EvaluableAst;
        public readonly ErrorInfo[] Errors;

        public ParseResult(EvaluableAst? evaluableAst, ErrorInfo[] errors)
        {
            EvaluableAst = evaluableAst;
            Errors = errors;
        }
    }

    internal struct ErrorInfo
    {
        public readonly ErrorLevel ErrorLevel;
        public readonly string Message;

        public ErrorInfo(ErrorLevel errorLevel, string message)
        {
            ErrorLevel = errorLevel;
            Message = message;
        }
    }

    internal enum ErrorLevel
    {
        Error,
        Warning
    }

    [Serializable]
    internal struct DeserializationModel
    {
        // ReSharper disable once InconsistentNaming
        public int ast_id;

        // ReSharper disable once InconsistentNaming
        public Diagnostic[] diagnostics;

        [Serializable]
        internal struct Diagnostic
        {
            // ReSharper disable once InconsistentNaming
            public string level;
            // ReSharper disable once InconsistentNaming
            public string message;
        }
    }

    internal static class ExprParser
    {
        public static ParseResult Parse(string expr, string[] idNames)
        {
            var jsonResult = Epp.CreateAst(expr, idNames);
            Debug.Log(jsonResult);
            var result = JsonUtility.FromJson<DeserializationModel>(jsonResult);

            var errors = new ErrorInfo[result.diagnostics.Length];
            var diagnostics = result.diagnostics;
            for (var i = 0; i < diagnostics.Length; ++i)
            {
                errors[i] = new ErrorInfo(
                    diagnostics[i].level == "Error"
                        ? ErrorLevel.Error
                        : ErrorLevel.Warning,
                    diagnostics[i].message
                );
            }

            return new ParseResult(
                result.ast_id == -1 ? null : new EvaluableAst(result.ast_id),
                errors
            );
        }
    }
}
