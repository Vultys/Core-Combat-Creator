using CCC.Attributes;
using CCC.Control;
using System;
using System.Collections;
using UnityEngine;

namespace CCC.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private float _respawnTime = 5f;

        [SerializeField] private float _healthToRestore = 0f;

        [SerializeField] private WeaponConfig _weapon;

        [SerializeField] private SphereCollider _coliider;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickable;
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            _coliider.enabled = shouldShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (_weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weapon);
            }
            if (_healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(_healthToRestore);
            }

            StartCoroutine(HideForSeconds(_respawnTime));
        }
    }
}