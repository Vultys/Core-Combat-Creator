using UnityEngine;

namespace CCC.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private float _range = 2f;
        [SerializeField] private float _damage = 10f;

        [Header("Prefabs")]
        [SerializeField] private GameObject _equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOverride = null; 
        
        public float Range => _range;

        public float Damage => _damage;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (_equippedPrefab != null)
            {
                Instantiate(_equippedPrefab, handTransform);
            }
            if(_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
        }
    }
}