using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int _startingLevel = 1;

        [SerializeField] private CharacterClass _characterClass;
    }
}
