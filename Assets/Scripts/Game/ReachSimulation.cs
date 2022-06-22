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

        private double _t;

        public override (string, double)[] GetInitialVariables()
        {
            return new (string, double)[] {("t", _t)};
        }

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            if (CollisionEventer == null)
            {
                throw new Exception("CollisionEventer is null");
            }

            CollisionEventer.OnCollisionEnter += OnCollide;
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (State == SimulationState.Running)
            {
                _t += Time.deltaTime;
            }

            VariableUpdate(new (string, double)[] {("t", _t)});
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            if (CollisionEventer != null) CollisionEventer.OnCollisionEnter -= OnCollide;
        }

        private void OnCollide(Collision2D collision)
        {
            if (State != SimulationState.Running) return;
            if (collision.gameObject == Target)
            {
                Success();
            }
        }

        public override void StartSimulation()
        {
            if (Ball == null)
            {
                throw new Exception("Ball is null");
            }

            base.StartSimulation();
            Ball.transform.position = BallInitialPosition;
            _t = 0;
        }

        public override void ResetSimulation()
        {
            if (Ball == null)
            {
                throw new Exception("Ball is null");
            }

            base.ResetSimulation();

            Ball.transform.position = BallInitialPosition;
            _t = 0;
            VariableUpdate(new (string, double)[] {("t", _t)});
        }
    }
}
