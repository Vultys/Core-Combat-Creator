using System;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            float[] levels = _lookupTable[characterClass][stat];

            if(levels.Length < level)
            {
                return 0;
            }

            return levels[level - 1];
        }

        private void BuildLookup()
        {
            if(_lookupTable != null) return;

            _lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach(var characterClass in _characterClasses)
            {
                Dictionary<Stat, float[]> innerDictionary = new Dictionary<Stat, float[]>();

                foreach (var progressionStat in characterClass.stats)
                {
                    innerDictionary[progressionStat.stat] = progressionStat.levels;
                }

                _lookupTable[characterClass.characterClass] = innerDictionary;
            }
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
