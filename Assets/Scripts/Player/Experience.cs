using UnityEngine;
using UnityEngine.Events;

namespace Players
{
    [RequireComponent(typeof(Player))]
    public class Experience : MonoBehaviour
    {
        private const int StartLevel = 1;
        private const int StepTargetLevel = 2;

        private int _targetLevel = 10;

        private int _level;
        private int _targetExperience;
        private int _currentExperience;

        public event UnityAction<int> LevelChanged;

        private void Awake()
        {
            _level = StartLevel;
            _targetExperience = 1;
            _currentExperience = 0;
            LevelChanged?.Invoke(_level);
        }

        public void Reset()
        {
            _level = StartLevel;
            LevelChanged?.Invoke(_level);
        }

        public void GetLevel(int level)
        {
            _level = level;
            LevelChanged?.Invoke(_level);
        }

        public void Add(int reward)
        {
            _currentExperience += reward;

            if (_currentExperience >= _targetExperience)
            {
                _level++;
                LevelChanged?.Invoke(_level);
                _currentExperience = 0;

                if (_level > _targetLevel)
                {
                    _targetLevel *= StepTargetLevel;
                    _targetExperience++;
                }
            }
        }
    }
}