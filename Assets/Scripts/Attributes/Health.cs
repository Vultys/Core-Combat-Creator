using CCC.Core;
using CCC.Saving;
using CCC.Stats;
using UnityEngine;

namespace CCC.Attributes
{    
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _healthPoints = 100f;

        private int _dieTriggerAnimatorHash = Animator.StringToHash("die");

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private void Start()
        {
            _healthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if(_healthPoints == 0)
            {
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
            float levelHealthPoints = GetComponent<BaseStats>().GetHealth();

            return (_healthPoints / levelHealthPoints) * 100;
        }
     
        private void Die()
        {
            if (_isDead) return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}