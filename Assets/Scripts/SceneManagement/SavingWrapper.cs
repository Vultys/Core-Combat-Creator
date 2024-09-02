using System;
using System.Collections;
using System.Collections.Generic;
using CCC.Saving;
using UnityEngine;

namespace CCC.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float _fadeInTime = 0.5f;

        [SerializeField] private SavingSystem _savingSystem;

        const string _defaultSaveFile = "save";

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return _savingSystem.LoadLastScene(_defaultSaveFile);
            yield return fader.FadeIn(_fadeInTime);
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Save()
        {
            _savingSystem.Save(_defaultSaveFile);
        }

        public void Load()
        {
            _savingSystem.Load(_defaultSaveFile);
        }

        public void Delete()
        {
            _savingSystem.Delete(_defaultSaveFile);
        }
    }
}