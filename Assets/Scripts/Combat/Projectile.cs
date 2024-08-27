using CCC.Core;
using UnityEngine;

namespace CCC.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private Health _target = null;

        private float _damage = 0f;

        private void Update()
        {
            if (_target == null) return;

            transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            _target = target;
            _damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            Health collidedObject = other.GetComponent<Health>();

            if (collidedObject == null) return;
            if (collidedObject != _target) return;

            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
