#nullable enable
using UnityEngine;
using Assets.Scripts.InGame;
using Assets.Scripts.Eventer;
using System;

namespace Assets.Scripts.Game
{

    public class ReachSimulation : Simulation
    {
        public CollisionEventer? CollisionEventer;
        public GameObject? Ball;
        public GameObject? Target;

        public Vector3 BallInitialPosition;
        
        private double _t = 0;

        public override (string, double)[] GetInitialVariables() {
            return new (string, double)[] { ("t", _t) };
        }

        public void Start() {
            if(CollisionEventer == null) {
                throw new Exception("CollisionEventer is null");
            }
            CollisionEventer.OnCollisionEnter += OnCollide;
        }

        public void Update() {
            if(State == SimulationState.Running) {
                _t += Time.deltaTime;
            }

            VariableUpdate(new (string, double)[]{("t", _t)});
        }

        public void OnDestroy() {
            if (CollisionEventer != null) CollisionEventer.OnCollisionEnter -= OnCollide;
        }

        private void OnCollide(Collision2D collision) {
            if(State == SimulationState.Running) {
                if(collision.gameObject == Target) {
                    Success();
                }
                else {
                    Failure();
                }
            }
        }

        public override void StartSimulation() {
            if(Ball == null) {
                throw new Exception("Ball is null");
            }

            base.StartSimulation();
            Ball.transform.position = BallInitialPosition;
            _t = 0;
        }

        public override void ResetSimulation() {
            if(Ball == null) {
                throw new Exception("Ball is null");
            }

            base.ResetSimulation();

            Ball.transform.position = BallInitialPosition;
        }
    }
}