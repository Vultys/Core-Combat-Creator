using CCC.Core;
using CCC.Movement;
using CCC.Saving;
using UnityEngine;

namespace CCC.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [Header("Settings")]
        [SerializeField] private float _timeBetweenAttacks = 1f;
        [SerializeField] private string _defaultWeaponName = "Unarmed";

        [Header("Components")]
        [SerializeField] private Mover _mover;
        [SerializeField] private ActionScheduler _scheduler;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        [SerializeField] private Weapon _defaultWeapon = null;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private Health _target; 

        private int _attackTriggerHash = Animator.StringToHash("attack");

        private int _stopAttackTriggerHash = Animator.StringToHash("stopAttack");
        
        private bool _isInRange => Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon?.Range;

        private Weapon _currentWeapon = null;

        private void Start()
        {
            if (_currentWeapon == null)
            {
                EquipWeapon(_defaultWeapon);
            }
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead)
            {
                return;
            }

            if (!_isInRange)
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel(); 
                AttackBehaviour();
            }
        }

        public void Attack(GameObject combatTarget)
        {
            _scheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            TriggerStop();
            _target = null;
            _mover.Cancel();
        }

        public bool CanAttack(GameObject combatTarget) 
        {
            if(combatTarget == null)
            {
                return false;
            }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void EquipWeapon(Weapon weapon)
        {
            weapon.Spawn(_rightHandTransform, _leftHandTransform, _animator);
            _currentWeapon = weapon;
        }

        public object CaptureState()
        {
            return _currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            EquipWeapon(Resources.Load<Weapon>(weaponName));
        }

        private void TriggerStop()
        {
            _animator.SetTrigger(_stopAttackTriggerHash);
            _animator.ResetTrigger(_attackTriggerHash);
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (_timeSinceLastAttack > _timeBetweenAttacks)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            _animator.SetTrigger(_attackTriggerHash);
            _animator.ResetTrigger(_stopAttackTriggerHash);
        }

        /// <summary>
        /// Animation event
        /// </summary>
        private void Hit()
        {
            if (_target == null) return;

            if (_currentWeapon.HasProjectile)
            {
                _currentWeapon.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target);
            }
            else
            {
                _target.TakeDamage(_currentWeapon.Damage);
            }
        }

        /// <summary>
        /// Animation event
        /// </summary>
        private void Shoot()
        {
            Hit();
        }
    }
}