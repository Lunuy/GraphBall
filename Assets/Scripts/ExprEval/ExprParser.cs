#nullable enable
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace Assets.Scripts.ExprEval
{
    internal struct ParseResult : IDisposable
    {
        [JsonProperty("ast_id")]
#pragma warning disable IDE0051 // Remove unused private members
        private int AstId
#pragma warning restore IDE0051 // Remove unused private members
        {
            set => EvaluableAst = value == -1 ? null : new EvaluableAst(value);
        }

        [JsonIgnore]
        public EvaluableAst? EvaluableAst;

        [JsonProperty("diagnostics")]
        public readonly ErrorInfo[] Errors;

        public ParseResult(EvaluableAst? evaluableAst, ErrorInfo[] errors)
        {
            EvaluableAst = evaluableAst;
            Errors = errors;
        }

        public void Dispose() => EvaluableAst?.Dispose();
    }

    internal struct ErrorInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("level")]
        public readonly ErrorLevel ErrorLevel;

        [JsonProperty("message")]
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

    internal static class ExprParser
    {
        public static ParseResult Parse(string expr, string[] idNames)
        {
            var jsonResult = Epp.CreateAst(expr, idNames);
            return JsonConvert.DeserializeObject<ParseResult>(jsonResult);
        }
    }
}
