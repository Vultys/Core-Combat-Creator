using UnityEngine;
using UnityEngine.Events;

namespace CCC.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onHit;

        public void OnHit()
        {
            _onHit.Invoke();
        }
    }
}
