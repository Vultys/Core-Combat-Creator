using CCC.Control;
using System;
using System.Collections;
using UnityEngine;

namespace CCC.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private float _respawnTime = 5f;

        [SerializeField] private Weapon _weapon;

        [SerializeField] private SphereCollider _coliider;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }

            return true;
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

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(_weapon);
            StartCoroutine(HideForSeconds(_respawnTime));
        }
    }
}