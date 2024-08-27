using UnityEngine;

namespace CCC.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        [SerializeField] private Transform _target = null;

        private void Update()
        {
            if (_target == null) return;

            transform.LookAt(GetAimLocation())  ;

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null)
            {
                return _target.position;
            }

            return _target.position + Vector3.up * targetCapsule.height / 2;
        }
    }
}
