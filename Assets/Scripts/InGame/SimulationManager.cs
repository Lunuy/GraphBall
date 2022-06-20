#nullable enable
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Graph.Selection;
using System;

namespace Assets.Scripts.InGame
{
    public delegate void SimulationEnd();

    public abstract class SimulationGraphEditManager : MonoBehaviour
    {
        public GraphEditManager? GraphEditManager;
        public Simulation? Simulation;

        virtual public void StartSimulation() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }

            GraphEditManager.IsEditable = false;
            Simulation.StartSimulation();
        }

        virtual public void ResetSimulation() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }
            if(Simulation == null) {
                throw new Exception("Simulation is null");
            }
            
            GraphEditManager.IsEditable = true;
            Simulation.ResetSimulation();
        }
    }
}