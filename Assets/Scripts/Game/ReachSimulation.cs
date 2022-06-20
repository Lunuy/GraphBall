#nullable enable
using UnityEngine;
using Assets.Scripts.InGame;
using Assets.Scripts.Eventer;
using System;

namespace Assets.Scripts.Game
{

    public class ReachSimulation : Simulation
    {
        override public event SimulationEnd OnSimulationFailure = delegate {  };
        override public event SimulationEnd OnSimulationSuccess = delegate {  };

        public CollisionEventer? CollisionEventer;
        public GameObject? Ball;
        public GameObject? Target;

        public Vector3 BallInitialPosition;

        public void Start() {
            if(CollisionEventer == null) {
                throw new Exception("CollisionEventer is null");
            }
            CollisionEventer.OnCollisionEnter_ += _onCollide;
        }

        public void OnDestroy() {
            if(CollisionEventer == null) {
                throw new Exception("CollisionEventer is null");
            }

            CollisionEventer.OnCollisionEnter_ -= _onCollide;
        }

        private void _onCollide(Collision2D collision) {
            if(IsRunning) {
                if(collision.gameObject == Target) {
                    OnSimulationSuccess();
                }
                else {
                    OnSimulationFailure();
                }
            }
        }

        override public void StartSimulation() {
            if(Ball == null) {
                throw new Exception("Ball is null");
            }

            base.StartSimulation();
            Ball.transform.position = BallInitialPosition;
        }

        override public void ResetSimulation() {
            base.ResetSimulation();
        }
    }
}