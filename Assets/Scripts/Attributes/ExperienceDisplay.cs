using System;
using UnityEngine;
using UnityEngine.UI;

namespace CCC.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private Text _experienceText;

        private Experience _experince;

        private void Awake()
        {
            _experince = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            _experienceText.text = string.Format("{0}", _experince.ExperiencePoints);
        }
    }
}
