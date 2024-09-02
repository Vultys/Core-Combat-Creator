using CCC.Core;
using CCC.Saving;
using CCC.Stats;
using UnityEngine;

namespace CCC.Attributes
{    
    public class Health : MonoBehaviour, ISaveable
    {
        private float _healthPoints = -1f;

        private int _dieTriggerAnimatorHash = Animator.StringToHash("die");

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private void Start()
        {
            if(_healthPoints >= 0f) return;
            _healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
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
            float levelHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);

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

            experience.GainPoints(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }
    }
}