using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach(var character in _characterClasses)
            {
                if (character.characterClass == characterClass)
                {
                    //eturn character.Stats[level - 1];
                }
            }

            return 0;
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat stats;
        }

        [Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
