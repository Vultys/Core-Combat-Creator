using UnityEngine;

namespace CCC.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                Destroy(gameObject);
            }
        }
    }
}