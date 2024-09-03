using GameDevTV.Utils;
using System;
using UnityEngine;

namespace CCC.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(0, 99)]
        [SerializeField] private int _startingLevel = 1;

        [SerializeField] private bool _shouldUseModifiers = false;

        [SerializeField] private CharacterClass _characterClass = CharacterClass.None;

        [SerializeField] private Progression _progression = null;

        [SerializeField] private GameObject _levelUpParticleEffect = null;

        private Experience _experience;

        private LazyValue<int> _currentLevel;

        public int CurrentLevel => _currentLevel.value;

        public event Action OnLevelUp;
        
        private void Awake()
        {
            _experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            _currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (_experience == null) return;

            _experience.OnGainingPoints += UpdateLevel;
        }

        private void OnDisable()
        {
            if (_experience == null) return;

            _experience.OnGainingPoints -= UpdateLevel;
        }

        public float GetStat(Stat stat) => (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);

        private int CalculateLevel()
        {
            if (_experience == null) return _startingLevel;

            float currentXP = _experience.ExperiencePoints;

            int penultimateLevel = _progression.GetLevels(Stat.ExperienceToLevelUp, CharacterClass.Player);

            for(int level = 1; level <= penultimateLevel; level++)
            {
                float xpToLevelUp = _progression.GetStat(Stat.ExperienceToLevelUp, _characterClass, level);
                if(xpToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > _currentLevel.value)
            {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        private void LevelUpEffect() => Instantiate(_levelUpParticleEffect, transform);

        private float GetAdditiveModifier(Stat stat)
        {
            if(!_shouldUseModifiers) return 0f;

            float result = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    result += modifier;
                }
            }

            return result;
        }

        private float GetBaseStat(Stat stat) => _progression.GetStat(stat, _characterClass, CurrentLevel);

        private float GetPercentageModifier(Stat stat)
        {
            if (!_shouldUseModifiers) return 0f;

            float result = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    result += modifier;
                }
            }

            return result;
        }
    }
}
