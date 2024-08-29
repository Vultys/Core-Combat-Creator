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

        [Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] private CharacterClass _class;
            [SerializeField] private float[] healths;
        }
    }
}
