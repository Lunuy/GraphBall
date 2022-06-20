#nullable enable
using UnityEngine;
using System;
using Assets.Scripts.UI;
using System.Collections.Generic;
using Assets.Scripts.ExprEval;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Graph.Selection
{
    public class GraphEditClient : MonoBehaviour
    {
        public delegate void ClickHandler(GraphEditClient graphEditClient);

        public event ClickHandler OnClick = delegate {  };

        public GraphEditManager? GraphEditManager;
        public EquationInputContext EquationInputContext = new EquationInputContext() { MaxCharacterCount = 30 };
        public List<string> allowedVariables = new List<string>() { "x" };

        public GraphSamplerComponent? GraphSampler;

        public void Start() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }
            if(EquationInputContext == null) {
                throw new Exception("EquationInputContext is null");
            }

            GraphEditManager.AddClient(this);
            EquationInputContext.OnInputChanged = _onInputChanged;
        }

        private void _onInputChanged(String exprInput) {
            if(GraphSampler == null) {
                throw new Exception("GraphSampler is null");
            }

            var variables = ArrayPool<string>.Rent(allowedVariables.Count);

            for(var i = 0; i < allowedVariables.Count; i++) {
                variables[i] = allowedVariables[i];
            }

            var parseResult = ExprParser.Parse(exprInput, variables);
            ArrayPool<string>.Return(variables);

            
                Debug.Log("A");

            if(parseResult.EvaluableAst == null) {
                var diagnosticsList = new List<string>();
                for(var i = 0; i < parseResult.Diagnostics.Length; i++) {
                    diagnosticsList.Add(parseResult.Diagnostics[i].ErrorLevel.ToString() + ": " + parseResult.Diagnostics[i].Message);
                }
                
                EquationInputContext.ErrorList = diagnosticsList;

                Debug.Log("B");

                return;
            } else {
                EquationInputContext.ErrorList = new List<string>();
            }

                Debug.Log("C");
            GraphSampler.Function = (t, x) =>
            {
                var variables = ArrayPool<(string, double)>.Rent(t.Length + 1);
                variables[0] = ("x", x);
                for(var i = 0; i < t.Length; i++) {
                    variables[i + 1] = (t[i].Item1, t[i].Item2);
                }

                var result = parseResult.EvaluableAst.Eval(variables);
                ArrayPool<(string, double)>.Return(variables);
                return result;
            };
        }

        public void OnMouseDown() {
            OnClick(this);
        }
    }
}