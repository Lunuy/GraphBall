#nullable enable
using UnityEngine;
using System;
using System.Collections.Generic;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.InGame
{
    public class SimulationRigidbodyManager : MonoBehaviour
    {
        public Simulation? simulation;

        private readonly List<SimulationRigidbodyClient> _clients = new();

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            if (simulation == null)
            {
                throw new Exception("Simulation is null");
            }

            simulation.OnSimulationStart += OnSimulationStart;
            simulation.OnSimulationPause += OnSimulationPause;
            simulation.OnSimulationResume += OnSimulationResume;
            simulation.OnSimulationReset += OnSimulationReset;
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            if (simulation == null) return;
            simulation.OnSimulationStart -= OnSimulationStart;
            simulation.OnSimulationPause -= OnSimulationPause;
            simulation.OnSimulationResume -= OnSimulationResume;
            simulation.OnSimulationReset -= OnSimulationReset;
        }

        public void AddClient(SimulationRigidbodyClient client)
        {
            _clients.Add(client);
        }

        private void OnSimulationStart()
        {
            for (var i = 0; i < _clients.Count; ++i)
            {
                var rigidBody = _clients[i].RigidBody!;
                rigidBody.simulated = true;
                rigidBody.velocity = Vector2.zero;
                rigidBody.angularVelocity = 0;
            }
        }

        private void OnSimulationPause()
        {
            for (var i = 0; i < _clients.Count; ++i)
            {
                _clients[i].RigidBody!.simulated = false;
            }
        }

        private void OnSimulationResume()
        {
            for (var i = 0; i < _clients.Count; ++i)
            {
                _clients[i].RigidBody!.simulated = true;
            }
        }

        private void OnSimulationReset()
        {
            for (var i = 0; i < _clients.Count; ++i)
            {
                var rigidBody = _clients[i].RigidBody!;
                rigidBody.simulated = false;
                rigidBody.velocity = Vector2.zero;
                rigidBody.angularVelocity = 0;
            }
        }
    }
}
