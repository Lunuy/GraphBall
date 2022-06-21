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

        public void SetUi(IReadOnlyDictionary<string, List<UnityEngine.Object>> dict) {
            _simulationControlUi = (Assets.Scripts.UI.SimulationControlUi)dict["GameUI"][1];
        }

        public void Start() {
            if(_simulationControlUi == null) {
                throw new Exception("GraphEditManager is null");
            }

            _simulationControlUi.OnPlayButtonClicked += _onPlayButtonClicked;
            _simulationControlUi.OnStopButtonClicked += _onStopButtonClicked;
            _simulationControlUi.OnPauseButtonClicked += _onPauseButtonClicked;
            _simulationControlUi.OnResumeButtonClicked += _onResumeButtonClicked;

            ResetSimulation();
        }

        private void _onPlayButtonClicked() {
            StartSimulation();
        }

        private void _onStopButtonClicked() {
            ResetSimulation();
        }

        private void _onPauseButtonClicked() {
            PauseSimulation();
        }

        private void _onResumeButtonClicked() {
            ResumeSimulation();
        }

        public void StartSimulation() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }

            GraphEditManager.IsEditable = false;
            Simulation.StartSimulation();
        }

        public void ResetSimulation() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }
            
            GraphEditManager.IsEditable = true;
            Simulation.ResetSimulation();
        }

        public void PauseSimulation() {
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }

            Simulation.PauseSimulation();
        }

        public void ResumeSimulation() {
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }

            Simulation.ResumeSimulation();
        }
    }
}
