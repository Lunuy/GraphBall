#nullable enable
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Graph.Selection;
using System;

namespace Assets.Scripts.InGame
{
    public delegate void SimulationEnd();

    public abstract class Simulation : MonoBehaviour
    {
        abstract public event SimulationEnd OnSimulationFailure;
        abstract public event SimulationEnd OnSimulationSuccess;

        public GraphEditManager? GraphEditManager;
        public bool IsRunning { get; private set; }

        virtual public void StartSimulation() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }

            GraphEditManager.IsEditable = false;
            IsRunning = true;
        }

        virtual public void ResetSimulation() {
            if(GraphEditManager == null) {
                throw new Exception("GraphEditManager is null");
            }
            
            GraphEditManager.IsEditable = true;
            IsRunning = false;
        }
    }
}