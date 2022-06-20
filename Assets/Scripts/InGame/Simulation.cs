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
        public event VariableUpdate OnVariableUpdate = (variables) => { };
        public bool IsRunning { get; private set; }
        virtual public void StartSimulation() {
            OnSimulationStart?.Invoke();
            IsRunning = true;
        }
        virtual public void ResetSimulation() {
            OnSimulationReset?.Invoke();
            IsRunning = false;
        }
        public void Success() {
            OnSimulationSuccess?.Invoke();
        }
        public void Failure() {
            OnSimulationFailure?.Invoke();
        }
    }
}