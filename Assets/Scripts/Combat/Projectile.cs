using CCC.Core;
using UnityEngine;

namespace CCC.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        
        private Health _target = null;

        private void Update()
        {
            if (_target == null) return;

            transform.LookAt(GetAimLocation())  ;

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void SetTarget(Health target) => _target = target;

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
    }
}
