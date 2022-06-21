#nullable enable
using UnityEngine;
using Assets.Scripts.Graph.Selection;
using System;
using Assets.Scripts.UI;
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

        public void Awake()
        {
            if (Simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            Simulation.OnSimulationSuccess += _onSimulationSuccess;
            Simulation.OnSimulationFailure += _onSimulationFailure;
        }

        public void OnDestroy()
        {
            if (Simulation != null)
            {
                Simulation.OnSimulationSuccess -= _onSimulationSuccess;
                Simulation.OnSimulationFailure -= _onSimulationFailure;
            }
        }

        private void _onSimulationSuccess()
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

        private void _onSimulationFailure()
        {
            if (_failedText == null)
            {
                throw new Exception("_failedText is null");
            }
            
            _failedText.SetActive(true);
        }
    }
}