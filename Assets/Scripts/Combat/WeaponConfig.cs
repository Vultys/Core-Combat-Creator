using CCC.Attributes;
using UnityEngine;

namespace CCC.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private float _range = 2f;
        [SerializeField] private float _damage = 5f;
        [SerializeField] private float _percentageBonus = 0f;
        [SerializeField] private bool _isRightHanded = true;

        [Header("Prefabs")]
        [SerializeField] private Weapon _equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOverride = null;
        [SerializeField] private Projectile _projectile = null;

        private const string _name = "Weapon";

        public float Range => _range;

        public float Damage => _damage;
        
        public float PercentageBonus => _percentageBonus;

        public bool HasProjectile => _projectile != null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (_equippedPrefab != null)
            {
                Transform hand = _isRightHanded ? rightHand : leftHand;
                Weapon weapon = Instantiate(_equippedPrefab, hand);
                weapon.gameObject.name = _name;
            }

            var aoc = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
            else if (aoc != null)
            {
                animator.runtimeAnimatorController = aoc.runtimeAnimatorController;
            }
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectile = Instantiate(_projectile, ChooseHand(rightHand, leftHand).position, Quaternion.identity);
            projectile.SetTarget(target, instigator, calculatedDamage);
        }

        private Transform ChooseHand(Transform rightHand, Transform leftHand) => _isRightHanded ? rightHand : leftHand;

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(_name);

            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(_name);
            }

            if(oldWeapon == null)
            {
                return;
            }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}