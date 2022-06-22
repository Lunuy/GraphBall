#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Assets.Scripts.InGame
{
    public class SimulationResultManager : MonoBehaviour
    {
        public Simulation? Simulation;
        private GameObject? _clearText;
        private GameObject? _failedText;

        public void SetUi(IReadOnlyDictionary<string, List<UnityEngine.Object>> dict) {
            _clearText = (GameObject)dict["GameUI"][2];
            _failedText = (GameObject)dict["GameUI"][3];
        }

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            Simulation.OnSimulationSuccess += OnSimulationSuccess;
            Simulation.OnSimulationFailure += OnSimulationFailure;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            if (Simulation == null) return;
            Simulation.OnSimulationSuccess -= OnSimulationSuccess;
            Simulation.OnSimulationFailure -= OnSimulationFailure;
        }

        private void OnSimulationSuccess()
        {
            if(_clearText == null)
            {
                throw new Exception("_clearText is null");
            }

            IEnumerator WaitAndActive()
            {
                yield return new WaitForSeconds(1.0f);
                _clearText.SetActive(true);
            }
            
            StartCoroutine(WaitAndActive());
        }

        private void OnSimulationFailure()
        {
            if (_failedText == null)
            {
                throw new Exception("_failedText is null");
            }
            
            _failedText.SetActive(true);
        }
    }
}
