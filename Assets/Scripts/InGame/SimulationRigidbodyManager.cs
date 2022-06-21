#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.InGame
{
    public class SimulationRigidbodyManager: MonoBehaviour {
        public Simulation? simulation;
        
        private List<SimulationRigidbodyClient> _clients = new List<SimulationRigidbodyClient>();

        public void Awake() {
            if(simulation == null) {
                throw new Exception("Simulation is null");
            }

            simulation.OnSimulationStart += _onSimulationStart;
            simulation.OnSimulationPause += _onSimulationPause;
            simulation.OnSimulationResume += _onSimulationResume;
            simulation.OnSimulationReset += _onSimulationReset;
        }

        public void OnDestroy() {
            if(simulation != null) {
                simulation.OnSimulationStart -= _onSimulationStart;
                simulation.OnSimulationPause -= _onSimulationPause;
                simulation.OnSimulationResume -= _onSimulationResume;
                simulation.OnSimulationReset -= _onSimulationReset;
            }
        }

        public void AddClient(SimulationRigidbodyClient client) {
            _clients.Add(client);
        }

        private void _onSimulationStart() {
            for(int i = 0; i < _clients.Count; i++) {
                var rigidBody = _clients[i].RigidBody!;
                rigidBody.simulated = true;
                rigidBody.velocity = Vector2.zero;
                rigidBody.angularVelocity = 0;
            }
        }

        private void _onSimulationPause() {
            for(int i = 0; i < _clients.Count; i++) {
                _clients[i].RigidBody!.simulated = false;
            }
        }

        private void _onSimulationResume() {
            for(int i = 0; i < _clients.Count; i++) {
                _clients[i].RigidBody!.simulated = true;
            }
        }

        private void _onSimulationReset() {
            for(int i = 0; i < _clients.Count; i++) {
                var rigidBody = _clients[i].RigidBody!;
                rigidBody.simulated = false;
                rigidBody.velocity = Vector2.zero;
                rigidBody.angularVelocity = 0;
            }
        }
    }
}