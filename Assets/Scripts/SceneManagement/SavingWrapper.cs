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

        private const string _defaultSaveFile = "save";

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
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

        private IEnumerator LoadLastScene()
        {
            yield return _savingSystem.LoadLastScene(_defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(_fadeInTime);
        }
    }
}