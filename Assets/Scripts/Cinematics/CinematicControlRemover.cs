using CCC.Control;
using CCC.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace CCC.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour 
    {
        [SerializeField] private PlayableDirector _playableDirector;

        [SerializeField] private GameObject _player;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector director)
        {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
            print(Random.Range(0, 4));
        }

        private void EnableControl(PlayableDirector director)
        {
            _player.GetComponent<PlayerController>().enabled = true;
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            _playableDirector.played -= DisableControl;
            _playableDirector.stopped -= EnableControl;
        }
    }

}