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
                if (character.Class == characterClass)
                {
                    return character.Healths[level - 1];
                }
            }

            return 0;
        }

        [Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass _class;
            [SerializeField] private float[] healths;

            public CharacterClass Class => _class;

            public float[] Healths => healths;
        }
    }
}
