using System;
using UnityEngine;
using UnityEngine.UI;

namespace CCC.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Text _healthText;

        private Health _health;

        private void Awake()
        {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            _healthText.text = String.Format("{0:0}/{1:0}", _health.HealthPoints, _health.GetMaxHealthPoints());
        }
    }
}
