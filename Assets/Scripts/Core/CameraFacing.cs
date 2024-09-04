using UnityEngine;

namespace CCC.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
