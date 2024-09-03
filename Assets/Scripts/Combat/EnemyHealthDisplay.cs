using System;
using UnityEngine;
using UnityEngine.UI;

namespace CCC.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private Text _healthText;

        private Fighter _fighter;

        private string _noTargetText = "N/A";

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            if(_fighter.Target != null)
            {
                _healthText.text = String.Format("{0:0}/{1:0}%", _fighter.Target.HealthPoints, _fighter.Target.GetMaxHealthPoints());
            }
            else
            {
                _healthText.text = _noTargetText;
            }
        }
    }
}
