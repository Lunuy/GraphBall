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

        override public (string, double)[] GetInitialVariables() {
            return new (string, double)[] { ("t", _t) };
        }

        public void Start() {
            if(CollisionEventer == null) {
                throw new Exception("CollisionEventer is null");
            }
            CollisionEventer.OnCollisionEnter_ += _onCollide;
        }

        public void Update() {
            _t += Time.deltaTime;

            VariableUpdate(new (string, double)[]{("t", _t)});
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
                    Success();
                }
                else {
                    Failure();
                }
            }
        }

        override public void StartSimulation() {
            if(Ball == null) {
                throw new Exception("Ball is null");
            }

            base.StartSimulation();
            Ball.transform.position = BallInitialPosition;
            _t = 0;
        }

        override public void ResetSimulation() {
            base.ResetSimulation();
        }
    }
}