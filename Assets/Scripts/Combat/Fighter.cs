using CCC.Core;
using CCC.Movement;
using CCC.Saving;
using CCC.Attributes;
using UnityEngine;
using CCC.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace CCC.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
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
        [SerializeField] private WeaponConfig _defaultWeapon = null;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private int _attackTriggerHash = Animator.StringToHash("attack");

        private int _stopAttackTriggerHash = Animator.StringToHash("stopAttack");
        
        private bool _isInRange => Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.value.Range;

        private LazyValue<WeaponConfig> _currentWeapon = null;

        private Health _target;

        public Health Target => _target;

        private void Awake()
        {
            _currentWeapon = new LazyValue<WeaponConfig>(SetDefaultWeapon);
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
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

        public bool CanAttack(GameObject combatTarget) 
        {
            if(combatTarget == null)
            {
                return false;
            }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        public void Cancel()
        {
            TriggerStop();
            _target = null;
            _mover.Cancel();
        }

        public object CaptureState()
        {
            return _currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            EquipWeapon(Resources.Load<WeaponConfig>(weaponName));
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return _currentWeapon.value.Damage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.PercentageBonus;
            }
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

        private void AttachWeapon(WeaponConfig weapon) => weapon.Spawn(_rightHandTransform, _leftHandTransform, _animator);

        private WeaponConfig SetDefaultWeapon()
        { 
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
        }
        /// <summary>
        /// Animation event
        /// </summary>
        private void Hit()
        {
            if (_target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (_currentWeapon.value.HasProjectile)
            {
                _currentWeapon.value.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
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