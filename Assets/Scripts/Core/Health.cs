using CCC.Saving;
using UnityEngine;

namespace CCC.Core
{    
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _healthPoints = 100f;

        private int _dieTriggerAnimatorHash = Animator.StringToHash("die");

        private bool _isDead = false;

        public bool IsDead => _isDead;

        public void TakeDamage(float damage)
        {
            _healthPoints = Mathf.Max(_healthPoints - damage, 0);
            if(_healthPoints == 0)
            {
                Die();
            }
        }    

        private void Die()
        {
            if(_isDead) return;

            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
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
    }
}