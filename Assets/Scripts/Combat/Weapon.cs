using UnityEngine;

namespace CCC.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private GameObject _weaponPrefab = null;

        [SerializeField] private AnimatorOverrideController _animatorOverride = null; 
        
        public void Spawn(Transform handTransform, Animator animator)
        {
            Instantiate(_weaponPrefab, handTransform);
            animator.runtimeAnimatorController = _animatorOverride;
        }
    }
}