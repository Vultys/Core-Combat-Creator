using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Audio
{
    public class AudioRandomizer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;

        [SerializeField] private AudioSource _player;

        public void PlayRandomAudio()
        {
            int nextAudioIndex = Random.Range(0, _audioClips.Length - 1);
            _player.clip = _audioClips[nextAudioIndex];
            _player.Play();
        }
    }

}
