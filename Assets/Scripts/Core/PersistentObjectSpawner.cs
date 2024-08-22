using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _persistentObjectPrefab;

        private static bool _hasSpawned = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (_hasSpawned) return;

            SpawnPersistentObjects();

            _hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            var createdObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(createdObject);
        }
    }
}