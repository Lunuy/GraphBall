#nullable enable
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Graph.Selection;
using System;
using Assets.Scripts.ExprEval;
using Assets.Scripts.Graph;
using Assets.Scripts.Utility;

namespace Assets.Scripts.InGame
{

    public abstract class SimulationClient : MonoBehaviour
    {
        public GraphSamplerComponent? GraphSampler;

        public Simulation? Simulation;

        public void Start() {
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }

            Simulation.OnVariableUpdate += _onVariableUpdate;
        }

        public void OnDestroy() {
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }

            Simulation.OnVariableUpdate -= _onVariableUpdate;
        }

        private void _onVariableUpdate((string, double)[] variables) {
            if(GraphSampler == null) {
                throw new Exception("GraphSampler is null");
            }
            
            GraphSampler.Variables = variables;
        }
    }
}