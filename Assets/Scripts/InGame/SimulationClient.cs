#nullable enable
using UnityEngine;
using System;
using Assets.Scripts.Graph;

namespace Assets.Scripts.InGame
{

    public class SimulationClient : MonoBehaviour
    {
        public GraphSamplerComponent? GraphSampler;

        public Simulation? Simulation;

        public void Start()
        {
            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            if (GraphSampler == null)
            {
                throw new Exception("GraphSampler is null");
            }

            Simulation.OnVariableUpdate += OnVariableUpdate;
            GraphSampler.Variables = Simulation.GetInitialVariables();
        }

        public void OnDestroy()
        {
            if (Simulation != null)
            {
                Simulation.OnVariableUpdate -= OnVariableUpdate;
            }
        }

        private void OnVariableUpdate((string, double)[] variables)
        {
            if (GraphSampler == null)
            {
                throw new Exception("GraphSampler is null");
            }

            GraphSampler.Variables = variables;
        }
    }
}
