using UnityEngine;

namespace CCC.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int _startingLevel = 1;

        [SerializeField] private CharacterClass _characterClass = CharacterClass.None;

        [SerializeField] private Progression _progression = null;

        private Experience _experience;

        private int _currentLevel = 0;

        public int CurrentLevel {
            get
            {
                if(_currentLevel < 1)
                {
                    _currentLevel = CalculateLevel();
                }

                return _currentLevel;
            }
        }

        private void Start()
        {
            _experience = GetComponent<Experience>();
            if (_experience == null) return;

            _experience.OnGainingPoints += UpdateLevel;
        }

        private void OnDestroy()
        {
            if (_experience == null) return;

            _experience.OnGainingPoints -= UpdateLevel;
        }

        public float GetStat(Stat stat) => _progression.GetStat(stat, _characterClass, CurrentLevel);

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return _startingLevel;

            float currentXP = experience.ExperiencePoints;

            int penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelUp, CharacterClass.Player);

            for(int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if(xpToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > _currentLevel)
            {
                _currentLevel = newLevel;
                print("Levelled Up!");
            }
        }
    }
}
