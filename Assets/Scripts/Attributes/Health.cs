using CCC.Core;
using CCC.Saving;
using CCC.Stats;
using UnityEngine;

namespace CCC.Attributes
{    
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regeneratePercentage = 70f;

        private float _healthPoints = -1f;

        private int _dieTriggerAnimatorHash = Animator.StringToHash("die");

        private bool _isDead = false;

        private BaseStats _baseStats = null;

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
            if( _baseStats != null )
            {
                _baseStats.OnLevelUp -= RegenerateHealth;
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if(_healthPoints == 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }  
        
        public object CaptureState()
        {
            return _healthPoints;
        }

        public void RestoreState(object state)
        {
            _healthPoints = (float)state;

            if(_healthPoints == 0)
            {
                Die();
            }
        }

        public float GetPercentage()
        {
            float levelHealthPoints = _baseStats.GetStat(Stat.Health);

            return (_healthPoints / levelHealthPoints) * 100;
        }
     
        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
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