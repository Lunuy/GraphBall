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

        public bool isFailed { get; private set; } = false;
        public bool isSuccess { get; private set; } = false;

        public abstract (string, double)[] GetInitialVariables();

        public SimulationState State { get; private set; } = SimulationState.Stopped;

        public virtual void StartSimulation()
        {
            OnSimulationStart.Invoke();
            State = SimulationState.Running;
        }

        public virtual void ResetSimulation()
        {
            isFailed = false;
            isSuccess = false;
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
            if(isFailed)
            {
                return;
            }
            isSuccess = true;
            OnSimulationSuccess.Invoke();
        }

        public void Failure()
        {
            if(isSuccess)
            {
                return;
            }
            isFailed = true;
            OnSimulationFailure.Invoke();
        }
    }
}
