using UnityEngine;

namespace CCC.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;

        /// <summary>
        /// Hooked up by Unity Event TakeDamage
        /// </summary>
        /// <param name="damage">Amount of damage to display</param>
        public void Spawn(float damage)
        {
            var instance = Instantiate<DamageText>(_damageTextPrefab, transform);
            instance.SetValue(damage);
        }
    }
}
