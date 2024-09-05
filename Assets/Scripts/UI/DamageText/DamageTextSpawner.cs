using System;
using UnityEngine;

namespace CCC.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText _damageTextPrefab;

        private void Start()
        {
            float damage = 0;
            Spawn(damage);
        }

        public void Spawn(float damage)
        {
            var instance = Instantiate<DamageText>(_damageTextPrefab, transform);
        }
    }
}
