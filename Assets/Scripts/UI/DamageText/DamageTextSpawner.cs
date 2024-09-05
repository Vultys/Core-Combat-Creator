using UnityEngine;

namespace CCC.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;

        public void Spawn(float damage)
        {
            var instance = Instantiate<DamageText>(_damageTextPrefab, transform);
            instance.SetValue(damage);
        }
    }
}
