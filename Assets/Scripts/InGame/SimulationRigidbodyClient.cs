#nullable enable
using UnityEngine;
using System;

namespace Assets.Scripts.InGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SimulationRigidbodyClient : MonoBehaviour
    {
        public SimulationRigidbodyManager? SimulationRigidbodyManager;
        public Rigidbody2D? RigidBody { get; set; }

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            if (SimulationRigidbodyManager == null)
            {
                throw new Exception("SimulationRigidbodyManager is null");
            }

            RigidBody = GetComponent<Rigidbody2D>();
            SimulationRigidbodyManager.AddClient(this);
        }
    }
}
