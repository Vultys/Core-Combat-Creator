using UnityEngine;

namespace CCC.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int _startingLevel = 1;

        [SerializeField] private CharacterClass _characterClass = CharacterClass.None;

        [SerializeField] private Progression _progression = null;

        public float GetStat(Stat stat) => _progression.GetStat(stat, _characterClass, _startingLevel);

        public int GetLevel()
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
    }
}
