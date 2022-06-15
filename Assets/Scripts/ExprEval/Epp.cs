#nullable enable
using System;
using System.Runtime.InteropServices;

namespace Assets.Scripts.ExprEval
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

    public static class Epp
    {
        public static string CreateAst(string expr, string[] idNames)
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
            for (var i = 0; i < idItemList.Length; ++i)
            {
                Marshal.StructureToPtr(idItemList[i], idItemListPtr + i * idItemSize, false);
            }
            
            var resultStr = Marshal.AllocCoTaskMem(1024 * 2);

            create_ast(
                cStrExpr,
                idItemList.Length,
                idItemListPtr,
                resultStr
            );

            var result = Marshal.PtrToStringUTF8(resultStr)!;

            //free marshal memory
            Marshal.FreeCoTaskMem(resultStr);
            Marshal.FreeCoTaskMem(idItemListPtr);

            for (var i = 0; i < idItemList.Length; ++i)
            {
                Marshal.FreeCoTaskMem(idItemList[i].Name);
            }
            Marshal.FreeCoTaskMem(cStrExpr);

            return result;
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

            var variablePairArray = new VariablePairArray
            {
                Count = variables.Length,
                Pairs = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(VariablePair)) * variables.Length)
            };

            for (var i = 0; i < variables.Length; ++i)
            {
                Marshal.StructureToPtr(marshaledVariables[i], variablePairArray.Pairs + i * Marshal.SizeOf(typeof(VariablePair)), false);
            }

            var result = eval_ast(astId, variablePairArray);

            //free marshal memory

            Marshal.FreeCoTaskMem(variablePairArray.Pairs);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < marshaledVariables.Length; ++i)
            {
                Marshal.FreeCoTaskMem(marshaledVariables[i].Name);
            }

            return result;
        }

        [DllImport("epp")]
        private static extern void create_ast(
            IntPtr input, // c str
            int idItemCount,
            IntPtr idItems, // IdItem*
            IntPtr output // c str
        );

        [DllImport("epp")]
        private static extern void dispose_ast(int astId);

        [DllImport("epp")]
        private static extern double eval_ast(int astId, VariablePairArray variables);
    }
}
