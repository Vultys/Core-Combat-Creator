using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Attributes
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float _experiencePoints;

        public void GainPoints(float experience)
        {
            _experiencePoints += experience;
        }
    }
}
