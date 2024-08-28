using System;
using System.Collections;
using UnityEngine;

namespace CCC.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private float _respawnTime = 5f;

        [SerializeField] private Weapon _weapon;

        [SerializeField] private SphereCollider _coliider;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                StartCoroutine(HideForSeconds(_respawnTime));
            }
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
    }
}