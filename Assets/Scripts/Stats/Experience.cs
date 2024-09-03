using CCC.Saving;
using System;
using UnityEngine;

namespace CCC.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints;

        public float ExperiencePoints => _experiencePoints;

        public event Action OnGainingPoints;

        public void GainPoints(float experience)
        {
            _experiencePoints += experience;
            OnGainingPoints?.Invoke();
        }

        public object CaptureState()
        {
            return _experiencePoints;
        }

        public void RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
    }
}
