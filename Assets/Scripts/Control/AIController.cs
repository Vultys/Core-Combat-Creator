using CCC.Attributes;
using CCC.Combat;
using CCC.Core;
using CCC.Movement;
using UnityEngine;

namespace CCC.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _chaseDistance = 5f;
        [SerializeField] private float _suspicionTime = 3f;
        [SerializeField] private float _waypointTolerance = 1f;
        [SerializeField] private float _waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] private float _patrolSpeedFraction = 0.2f;
        [SerializeField] private PatrolPath _patrolPath;

        [Header("Components")]
        [SerializeField] private Fighter _fighter;
        [SerializeField] private Health _health;
        [SerializeField] private Mover _mover;

        private bool _isInRange => Vector3.Distance(transform.position, _player.transform.position) < _chaseDistance;

        private Vector3 _currentWaypoint => _patrolPath.GetWaypoint(_currentWaypointIndex); 
       
        private int _currentWaypointIndex = 0;

        private GameObject _player; 
        
        private Vector3 _guardPosition;

        private float _timeSinceLastSawPlayer = Mathf.Infinity;

        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {
            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead)
            {
                return;
            }

            if (_isInRange && _fighter.CanAttack(_player))
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

        private void UpdateTimers()
        {
            _timeSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition;

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
        }

        /// <summary>
        /// Callback to draw gizmos only if the object is selected.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}