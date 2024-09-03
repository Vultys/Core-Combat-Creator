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

        private void OnEnable()
        {
            _playableDirector.played += DisableControl;
            _playableDirector.stopped += EnableControl;
        }
        private void OnDisable()
        {
            _playableDirector.played -= DisableControl;
            _playableDirector.stopped -= EnableControl;
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
    }
}