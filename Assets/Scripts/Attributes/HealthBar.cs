using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health = null;

        [SerializeField] private RectTransform _foreground = null;

        private void Update()
        {
            _foreground.localScale = new Vector3(_health.GetFraction(), 1f, 1f);
        }
    }
}
