#nullable enable
using UnityEngine;

namespace Assets.Scripts.InGame
{
    public delegate void SimulationEvent();

    public delegate void VariableUpdate((string, double)[] variables);

    public abstract class Simulation : MonoBehaviour
    {
        public enum SimulationState
        {
            Stopped,
            Running,
            Paused
        }

        public event SimulationEvent OnSimulationStart = () => { };
        public event SimulationEvent OnSimulationPause = () => { };
        public event SimulationEvent OnSimulationResume = () => { };
        public event SimulationEvent OnSimulationReset = () => { };

        public event SimulationEvent OnSimulationFailure = () => { };
        public event SimulationEvent OnSimulationSuccess = () => { };
        public event VariableUpdate OnVariableUpdate = _ => { };

        public abstract (string, double)[] GetInitialVariables();

        public SimulationState State { get; private set; } = SimulationState.Stopped;

        public virtual void StartSimulation()
        {
            OnSimulationStart.Invoke();
            State = SimulationState.Running;
        }

        public virtual void ResetSimulation()
        {
            OnSimulationReset.Invoke();
            State = SimulationState.Stopped;
        }

        public virtual void PauseSimulation()
        {
            OnSimulationPause.Invoke();
            State = SimulationState.Paused;
        }

        public virtual void ResumeSimulation()
        {
            OnSimulationResume.Invoke();
            State = SimulationState.Running;
        }

        public void VariableUpdate((string, double)[] variables)
        {
            OnVariableUpdate.Invoke(variables);
        }

        public void Success()
        {
            OnSimulationSuccess.Invoke();
        }

        public void Failure()
        {
            OnSimulationFailure.Invoke();
        }
    }
}
