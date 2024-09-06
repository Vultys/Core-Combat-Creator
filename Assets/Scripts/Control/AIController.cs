using CCC.Attributes;
using CCC.Combat;
using CCC.Core;
using CCC.Movement;
using GameDevTV.Utils;
using System;
using UnityEngine;

namespace CCC.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _chaseDistance = 5f;
        [SerializeField] private float _suspicionTime = 3f;
        [SerializeField] private float _agroCullDownTime = 5f;
        [SerializeField] private float _waypointTolerance = 1f;
        [SerializeField] private float _waypointDwellTime = 3f;
        [SerializeField] private float _shoutDistance = 5f;
        [Range(0,1)]
        [SerializeField] private float _patrolSpeedFraction = 0.2f;
        [SerializeField] private PatrolPath _patrolPath;

        [Header("Components")]
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;

        private bool _isAggrevated => (Vector3.Distance(transform.position, _player.transform.position) < _chaseDistance) || _timeSinceAggrevated < _agroCullDownTime;

        private Vector3 _currentWaypoint => _patrolPath.GetWaypoint(_currentWaypointIndex); 
       
        private int _currentWaypointIndex = 0;

        private GameObject _player; 
        
        private LazyValue<Vector3> _guardPosition;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private float _timeSinceAggrevated = Mathf.Infinity;
        

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            _guardPosition.ForceInit();
        }

        private void Update()
        {
            if (_health.IsDead)
            {
                return;
            }

            if (_isAggrevated && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSawPlayer < _suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        /// <summary>
        /// Callback to draw gizmos only if the object is selected.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }

        public void Aggrevate()
        {
            _timeSinceAggrevated = 0f;
        }

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition.value;

            if(_patrolPath != null)
            {
                if(AtWayPoint())
                {
                    _timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = _currentWaypoint;
            }

            if (_timeSinceArrivedAtWaypoint > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, _currentWaypoint);
            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _currentWaypointIndex = _patrolPath.GetNext(_currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSawPlayer = 0f;
            _fighter.Attack(_player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _shoutDistance, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                hit.collider.GetComponent<AIController>()?.Aggrevate();
            }
        }

        private Vector3 GetGuardPosition() => transform.position;
    }
}