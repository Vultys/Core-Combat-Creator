using CCC.Core;
using CCC.Saving;
using CCC.Stats;
using GameDevTV.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CCC.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        [SerializeField] private float regeneratePercentage = 70f;

        [SerializeField] private TakeDamageEvent _takeDamage;

        private int _dieTriggerAnimatorHash = Animator.StringToHash("die");

        private BaseStats _baseStats = null;

        private LazyValue<float> _healthPoints;

        public float HealthPoints => _healthPoints.value;

        private bool _isDead = false;
        public bool IsDead => _isDead;

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            if (_baseStats != null)
            {
                _baseStats.OnLevelUp += RegenerateHealth;
            }
        }

        private void OnDisable()
        {
            if(_baseStats != null)
            {
                _baseStats.OnLevelUp -= RegenerateHealth;
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0); 
            
            if (_healthPoints.value == 0)
            {
                AwardExperience(instigator);
                Die();
            }
            else
            {
                _takeDamage?.Invoke(damage);
            }
        }

        public float GetPercentage() => GetFraction() * 100;

        public float GetFraction()
        {
            float levelHealthPoints = GetMaxHealthPoints();

            return (_healthPoints.value / levelHealthPoints);
        }

        public float GetMaxHealthPoints()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return _healthPoints.value;
        }

        public void RestoreState(object state)
        {
            _healthPoints.value = (float)state;

            if (_healthPoints.value == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger(_dieTriggerAnimatorHash);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainPoints(_baseStats.GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regeneratingHealth = _baseStats.GetStat(Stat.Health) * (regeneratePercentage / 100);
            _healthPoints.value = Mathf.Max(regeneratingHealth, _healthPoints.value);
        }

        private float GetInitialHealth() => _baseStats.GetStat(Stat.Health);
    }
}