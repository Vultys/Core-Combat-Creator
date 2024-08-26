using UnityEngine;

namespace CCC.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private float _range = 2f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private bool _isRightHanded = true;

        [Header("Prefabs")]
        [SerializeField] private GameObject _equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOverride = null; 
        
        public float Range => _range;

        public float Damage => _damage;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_equippedPrefab != null)
            {
                Transform hand = _isRightHanded ? rightHand : leftHand;
                Instantiate(_equippedPrefab, hand);
            }
            if(_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
        }
    }
}