using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace CCC.Cinematics
{

    public class CinematicTrigger : MonoBehaviour
    {
        private bool _wasTriggered = false;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if(!_wasTriggered && other.gameObject.tag == "Player")
            {
                _wasTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}