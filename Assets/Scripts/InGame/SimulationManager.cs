#nullable enable
using UnityEngine;
using Assets.Scripts.Graph.Selection;
using System;
using Assets.Scripts.UI;
using System.Collections.Generic;

namespace Assets.Scripts.InGame
{
    public delegate void SimulationEnd();

    public class SimulationManager : MonoBehaviour
    {
        public GraphEditManager? GraphEditManager;
        public Simulation? Simulation;
        private SimulationControlUi? _simulationControlUi;

        // ReSharper disable once UnusedMember.Global
        public void SetUi(IReadOnlyDictionary<string, List<UnityEngine.Object>> dict)
        {
            _simulationControlUi = (SimulationControlUi) dict["GameUI"][1];
        }

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            if (_simulationControlUi == null)
            {
                throw new Exception("GraphEditManager is null");
            }

            _simulationControlUi.OnPlayButtonClicked += OnPlayButtonClicked;
            _simulationControlUi.OnStopButtonClicked += OnStopButtonClicked;
            _simulationControlUi.OnPauseButtonClicked += OnPauseButtonClicked;
            _simulationControlUi.OnResumeButtonClicked += OnResumeButtonClicked;

            ResetSimulation();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            if (_simulationControlUi == null) return;
            _simulationControlUi.OnPlayButtonClicked -= OnPlayButtonClicked;
            _simulationControlUi.OnStopButtonClicked -= OnStopButtonClicked;
            _simulationControlUi.OnPauseButtonClicked -= OnPauseButtonClicked;
            _simulationControlUi.OnResumeButtonClicked -= OnResumeButtonClicked;
        }

        private void OnPlayButtonClicked()
        {
            StartSimulation();
        }

        private void OnStopButtonClicked()
        {
            ResetSimulation();
        }

        private void OnPauseButtonClicked()
        {
            PauseSimulation();
        }

        private void OnResumeButtonClicked()
        {
            ResumeSimulation();
        }

        public void StartSimulation()
        {
            if (GraphEditManager == null)
            {
                throw new Exception("GraphEditManager is null");
            }

            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            GraphEditManager.IsEditable = false;
            Simulation.StartSimulation();
        }

        public void ResetSimulation()
        {
            if (GraphEditManager == null)
            {
                throw new Exception("GraphEditManager is null");
            }

            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            GraphEditManager.IsEditable = true;
            Simulation.ResetSimulation();
        }

        public void PauseSimulation()
        {
            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            Simulation.PauseSimulation();
        }

        public void ResumeSimulation()
        {
            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            Simulation.ResumeSimulation();
        }
    }
}
