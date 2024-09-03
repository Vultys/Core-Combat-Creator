﻿using CCC.Attributes;
using UnityEngine;

namespace CCC.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        [SerializeField] private bool _isHoming = false;

        [SerializeField] private float _maxLifeTime = 10f;

        [SerializeField] private float _lifeAfterImpact = 2f;

        [SerializeField] private GameObject _hitEffect = null;

        [SerializeField] private GameObject[] _destroyOnHit = null;

        private Health _target = null;

        private GameObject _instigator = null;

        private float _damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            if (_isHoming && !_target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            _target = target;
            _damage = damage;
            _instigator = instigator;

            Destroy(gameObject, _maxLifeTime);
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
            if (collidedObject.IsDead) return;
            if (collidedObject != _target) return;

            _target.TakeDamage(_instigator, _damage);

            _speed = 0f;

            if (_hitEffect != null)
            {
                Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
            }

            for (int i = 0; i < _destroyOnHit.Length; i++)
            {
                Destroy(_destroyOnHit[i]);
            }

            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}
