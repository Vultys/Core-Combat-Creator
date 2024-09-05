using UnityEngine;
using UnityEngine.UI;

namespace CCC.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text _damageText = null;

        public void DestroyText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float value)
        {
            _damageText.text = string.Format("{0:0}", value);
        }
    }
}
