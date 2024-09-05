using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health _health = null;

        [SerializeField] private RectTransform _foreground = null;

        [SerializeField] private Canvas _rootCanvas = null;

        private float _healthFraction = 0f;

        private void Update()
        {
            _healthFraction = _health.GetFraction();

            if (Mathf.Approximately(_healthFraction, 0f) || Mathf.Approximately(_healthFraction, 1f))
            {
                _rootCanvas.enabled = false;
                return;
            }

            _rootCanvas.enabled = true;
            _foreground.localScale = new Vector3(_healthFraction, 1f, 1f);
        }
    }
}
