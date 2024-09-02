using CCC.Saving;
using UnityEngine;

namespace CCC.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _experiencePoints;

        public float ExperiencePoints => _experiencePoints;

        public void GainPoints(float experience)
        {
            _experiencePoints += experience;
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
