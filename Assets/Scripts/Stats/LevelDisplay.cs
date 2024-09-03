using UnityEngine;
using UnityEngine.UI;

namespace CCC.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private Text _levelText;

        private BaseStats _playerStats;

        private void Awake()
        {
            _playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            _levelText.text = string.Format("{0}", _playerStats.CurrentLevel);
        }
    }
}
