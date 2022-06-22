#nullable enable
using UnityEngine;
using System;
using Assets.Scripts.Graph;

namespace Assets.Scripts.InGame
{

    // ReSharper disable once UnusedMember.Global
    public class SimulationClient : MonoBehaviour
    {
        public GraphSamplerComponent? GraphSampler;

        public Simulation? Simulation;

        // ReSharper disable once UnusedMember.Local
        private void Start()
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

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
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
