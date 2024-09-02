using System.Collections;
using System.Collections.Generic;
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
    }
}
