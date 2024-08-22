using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace CCC.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        public enum DestinationIdentifier
        {
            A, B, C, D, E, F, G
        }

        [SerializeField] private int _sceneToLoad = -1;

        [SerializeField] private Transform _spawnPoint;

        [SerializeField] private DestinationIdentifier _destination;

        [SerializeField] private float _fadeOutDuration = 1f;

        [SerializeField] private float _fadeWaitDuration = 0.5f;

        [SerializeField] private float _fadeInDuration = 2f;

        public Transform SpawnPoint => _spawnPoint;
        
        public DestinationIdentifier Destination => _destination;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {   
            if(_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeOutDuration);

            SavingWrapper saving = FindObjectOfType<SavingWrapper>();
            saving.Save();

            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            saving.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            saving.Save();
            
            yield return new WaitForSeconds(_fadeWaitDuration);
            yield return fader.FadeIn(_fadeInDuration);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            player.transform.position = otherPortal.SpawnPoint.position;
            player.transform.rotation = otherPortal.SpawnPoint.rotation;
            agent.enabled = true;
        }

        private Portal GetOtherPortal()
        {
            var portals = GameObject.FindGameObjectsWithTag("Portal");
            foreach(var portal in portals)
            {
                var askedPortal = portal.GetComponent<Portal>();
                if (portal != gameObject && askedPortal.Destination == _destination)
                {
                    return askedPortal;
                }
            }
            return null;
        }
    }
}