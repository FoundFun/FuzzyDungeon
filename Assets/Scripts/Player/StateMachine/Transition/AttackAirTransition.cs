using UnityEngine;
using UnityEngine.UI;

namespace Players
{
    [RequireComponent(typeof(Demon))]
    public class AttackAirTransition : Transition
    {
        private const float DelayAttack = 10;

        private float _currentTime = DelayAttack;
        private float _targetFill = 0;

        private Demon _demon;
        private Image _darkFill;

        private void Awake()
        {
            _demon = GetComponent<Demon>();
        }

        private void Start()
        {
            _darkFill.fillAmount = _targetFill;
        }

        private void Update()
        {
            if (_currentTime > DelayAttack && Input.GetKey(KeyCode.Q)
                && _demon.AttackState == false && Time.timeScale != 0)
            {
                _currentTime = 0;
                _darkFill.fillAmount = 1;
                NeedTransit = true;
            }

            if (_darkFill.fillAmount != _targetFill)
            {
                _darkFill.fillAmount = Mathf.MoveTowards(_darkFill.fillAmount,
                    _targetFill, Time.deltaTime / DelayAttack);
            }

            _currentTime += Time.deltaTime;
        }

        public void Init(DarkFill darkFill)
        {
            _darkFill = darkFill.GetComponent<Image>();
        }
    }
}