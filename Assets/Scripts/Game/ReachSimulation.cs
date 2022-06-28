#nullable enable
using UnityEngine;
using Assets.Scripts.InGame;
using Assets.Scripts.Eventer;
using System;
using Assets.Scripts.Utility;

namespace Assets.Scripts.Game
{

    public class ReachSimulation : Simulation
    {
        public CollisionEventer? CollisionEventer;
        public TriggerEventer? TriggerEventer;
        public GameObject? Ball;
        public GameObject? Target;

        private Vector3 _ballInitialPosition;
        private GameObject[]? _avoidTargets;

        private double _t;

        public override (string, double)[] GetInitialVariables()
        {
            return new (string, double)[] {("t", _t)};
        }

        // ReSharper disable once UnusedMember.Local
        private void Awake() {
            if(Ball == null) {
                throw new Exception("Ball is null");
            }
            _ballInitialPosition = Ball.transform.position;
        }

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            if (CollisionEventer != null)
            {
                CollisionEventer.OnCollisionEnter += OnCollide;
            }
            if(TriggerEventer != null)
            {
                TriggerEventer.OnTriggerEnter += OnTrigger;
            }
            if(CollisionEventer == null && TriggerEventer == null)
            {
                throw new Exception("CollisionEventer and TriggerEventer is null");
            }


            _avoidTargets = GameObject.FindGameObjectsWithTag("avoid");
        }

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (State == SimulationState.Running)
            {
                _t += Time.deltaTime;
            }

            var variables = ArrayPool<(string, double)>.Rent(1);
            variables[0] = ("t", _t);
            VariableUpdate(variables);
            ArrayPool<(string, double)>.Return(variables);
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            if (CollisionEventer != null) CollisionEventer.OnCollisionEnter -= OnCollide;
            if (TriggerEventer != null)
            {
#pragma warning disable CS8601
                TriggerEventer.OnTriggerEnter -= OnTrigger;
#pragma warning restore CS8601
            }
        }

        private void OnCollide(Collision2D collision)
        {
            if (State != SimulationState.Running) return;
            if (collision.gameObject == Target)
            {
                Success();
                return;
            }
            if (_avoidTargets != null)
            {
                foreach (var target in _avoidTargets)
                {
                    if (collision.gameObject == target)
                    {
                        Failure();
                        return;
                    }
                }
            }
        }

        private void OnTrigger(Collider2D collision)
        {
            if (State != SimulationState.Running) return;
            if (collision.gameObject == Target)
            {
                Success();
                return;
            }
            if (_avoidTargets != null)
            {
                foreach (var target in _avoidTargets)
                {
                    if (collision.gameObject == target)
                    {
                        Failure();
                        return;
                    }
                }
            }
        }

        public override void StartSimulation()
        {
            if (Ball == null)
            {
                throw new Exception("Ball is null");
            }

            base.StartSimulation();
            Ball.transform.position = _ballInitialPosition;
            _t = 0;
        }

        public override void ResetSimulation()
        {
            if (Ball == null)
            {
                throw new Exception("Ball is null");
            }

            base.ResetSimulation();

            Ball.transform.position = _ballInitialPosition;
            _t = 0;
            VariableUpdate(new (string, double)[] {("t", _t)});
        }
    }
}
