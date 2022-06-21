#nullable enable
using UnityEngine;

namespace Assets.Scripts.InGame
{
    public delegate void SimulationEvent();
    public delegate void VariableUpdate((string, double)[] variables);

    public abstract class Simulation : MonoBehaviour
    {
        public event SimulationEvent OnSimulationStart = () => { };
        public event SimulationEvent OnSimulationReset = () => { };
        public event SimulationEvent OnSimulationFailure = () => { };
        public event SimulationEvent OnSimulationSuccess = () => { };
        public event VariableUpdate OnVariableUpdate = _ => { };
        
        public abstract (string, double)[] GetInitialVariables();

        public bool IsRunning { get; private set; }
        public virtual void StartSimulation() {
            OnSimulationStart.Invoke();
            IsRunning = true;
        }
        public virtual void ResetSimulation() {
            OnSimulationReset.Invoke();
            IsRunning = false;
        }
        public void VariableUpdate((string, double)[] variables) {
            OnVariableUpdate.Invoke(variables);
        }
        public void Success() {
            OnSimulationSuccess.Invoke();
        }
        public void Failure() {
            OnSimulationFailure.Invoke();
        }
    }
}
