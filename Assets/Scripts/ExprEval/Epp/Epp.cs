#nullable enable
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Assets.Scripts.ExprEval.Epp
{
    internal struct VariablePair
    {
        public IntPtr Name;
        public double Value;
    }

    internal struct VariablePairArray
    {
        public int Count;
        public IntPtr Pairs; // VariablePair*
    }

    internal struct IdItem
    {
        public IntPtr Name; //c str
    }

#pragma warning disable 0649
    internal unsafe struct LowParseResult
    {
        public int AstId;
        public LowDiagnostic* Diagnostics;
        public int DiagnosticsLength;
    }

    internal struct LowDiagnostic
    {
        public ErrorLevel Level;
        public IntPtr Message; // c str
        public int MessageLength;
    }
#pragma warning restore 0649

    public readonly struct ParseResult
    {
        public readonly int AstId;
        public readonly Diagnostic[] Diagnostics;

        public ParseResult(int astId, Diagnostic[] diagnostics)
        {
            AstId = astId;
            Diagnostics = diagnostics;
        }
    }

    public struct Diagnostic
    {
        public readonly ErrorLevel ErrorLevel;

        public readonly string Message;

        public Diagnostic(ErrorLevel errorLevel, string message)
        {
            ErrorLevel = errorLevel;
            Message = message;
        }
    }

    public enum ErrorLevel
    {
        Error = 0,
        Warning = 1,
        Note = 2
    }

    public static class Epp
    {
        public static ParseResult CreateAst(string expr, string[] idNames)
        {
            var cStrExpr = Marshal.StringToCoTaskMemUTF8(expr);
            Span<IdItem> idItemList = stackalloc IdItem[idNames.Length];
            for (var i = 0; i < idNames.Length; ++i)
            {
                idItemList[i] = new IdItem
                {
                    Name = Marshal.StringToCoTaskMemUTF8(idNames[i])
                };
            }
            
            var idItemSize = Marshal.SizeOf(typeof(IdItem));

            var idItemListPtr = Marshal.AllocCoTaskMem(idItemSize * idItemList.Length);
            unsafe
            {
                var idItemListPtrCast = (IdItem*) idItemListPtr;
                for (var i = 0; i < idItemList.Length; ++i)
                {
                    idItemListPtrCast[i] = idItemList[i];
                    //Marshal.StructureToPtr(idItemList[i], idItemListPtr + i * idItemSize, false);
                }
            }
            

            var result = create_ast(
                cStrExpr,
                idItemList.Length,
                idItemListPtr
            );

            var diagnostics = new Diagnostic[result.DiagnosticsLength];
            for (var i = 0; i < result.DiagnosticsLength; ++i)
            {
                unsafe
                {
                    var buffer = new byte[result.Diagnostics[i].MessageLength];
                    Marshal.Copy(result.Diagnostics[i].Message, buffer, 0, result.Diagnostics[i].MessageLength);

                    diagnostics[i] = new Diagnostic(
                        result.Diagnostics[i].Level,
                        Encoding.UTF8.GetString(buffer)
                    );
                }
            }

            //free marshal memory
            Marshal.FreeCoTaskMem(idItemListPtr);

            for (var i = 0; i < idItemList.Length; ++i)
            {
                Marshal.FreeCoTaskMem(idItemList[i].Name);
            }
            Marshal.FreeCoTaskMem(cStrExpr);

            return new ParseResult(
                result.AstId,
                diagnostics
            );
        }

        public static void DisposeAst(int astId) => dispose_ast(astId);

        public static double EvalAst(int astId, (string, double)[] variables)
        {
            Span<VariablePair> marshaledVariables = stackalloc VariablePair[variables.Length];
            for (var i = 0; i < variables.Length; ++i)
            {
                marshaledVariables[i].Name = Marshal.StringToCoTaskMemUTF8(variables[i].Item1);
                marshaledVariables[i].Value = variables[i].Item2;
            }

            var variablePairSize = Marshal.SizeOf(typeof(VariablePair));

            var variablePairArray = new VariablePairArray
            {
                Count = variables.Length,
                Pairs = Marshal.AllocCoTaskMem(variablePairSize * variables.Length)
            };

            for (var i = 0; i < variables.Length; ++i)
            {
                unsafe
                {
                    var pairs = (VariablePair*)variablePairArray.Pairs.ToPointer();
                    pairs[i] = marshaledVariables[i];
                }
                //Marshal.StructureToPtr(marshaledVariables[i], variablePairArray.Pairs + i * variablePairSize, false);
            }

            var result = eval_ast(astId, variablePairArray);

            //free marshal memory

            Marshal.FreeCoTaskMem(variablePairArray.Pairs);
            
            for (var i = 0; i < marshaledVariables.Length; ++i)
            {
                Marshal.FreeCoTaskMem(marshaledVariables[i].Name);
            }

            return result;
        }

        [DllImport("epp")]
        private static extern LowParseResult create_ast(
            IntPtr input, // c str
            int idItemCount,
            IntPtr idItems // IdItem*
        );

        [DllImport("epp")]
        private static extern void dispose_ast(int astId);

        [DllImport("epp")]
        private static extern double eval_ast(int astId, VariablePairArray variables);
    }
}
