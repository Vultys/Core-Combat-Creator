using CCC.Combat;
using CCC.Core;
using UnityEngine;
using UnityEngine.AI;
using CCC.Saving;
using System.Collections.Generic;
namespace CCC.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [Header("Settings")]
        [SerializeField] private float _maxSpeed = 6;

        [Header("Components")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Animator _animator; 
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private ActionScheduler _scheduler;

        private int _animationForwardSpeedHash = Animator.StringToHash("forwardSpeed");

        private void Update()
        {
            _agent.enabled = !_health.IsDead;

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _scheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _agent.destination = destination;
            _agent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
            _agent.isStopped = false;
        }

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);

            float speed = localVelocity.z;

            _animator.SetFloat(_animationForwardSpeedHash, speed);
        }

        [System.Serializable]
        public struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            
            return data;
        }

        public void RestoreState(object state)
        {
           MoverSaveData data = (MoverSaveData) state;
            _agent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            _agent.enabled = true;
        }
    }
}