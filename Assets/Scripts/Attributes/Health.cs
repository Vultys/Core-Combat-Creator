using CCC.Core;
using CCC.Saving;
using CCC.Stats;
using UnityEngine;

namespace CCC.Attributes
{    
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regeneratePercentage = 70f;

        private int _dieTriggerAnimatorHash = Animator.StringToHash("die");

        private BaseStats _baseStats = null;

        private float _healthPoints = -1f;

        public float HealthPoints => _healthPoints;

        private bool _isDead = false;
        public bool IsDead => _isDead;

        private void Start()
        {
            _baseStats = GetComponent<BaseStats>();
            if (_baseStats != null)
            {
                _baseStats.OnLevelUp += RegenerateHealth;
            }

            if (_healthPoints >= 0f) return;
            _healthPoints = _baseStats.GetStat(Stat.Health);
        }

        private void OnDestroy()
        {
            if(_baseStats != null)
            {
                _baseStats.OnLevelUp -= RegenerateHealth;
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name + " took damage: " + damage);

            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if(_healthPoints == 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }  

        public float GetPercentage()
        {
            float levelHealthPoints = GetMaxHealthPoints();

            return (_healthPoints / levelHealthPoints) * 100;
        }
        
        public float GetMaxHealthPoints()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float)state;

            if (_healthPoints == 0)
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
            _healthPoints = Mathf.Max(regeneratingHealth, _healthPoints);
        }
    }
}