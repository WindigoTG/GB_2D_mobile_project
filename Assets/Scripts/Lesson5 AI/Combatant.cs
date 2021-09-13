using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

namespace AI
{
    public abstract class Combatant
    {
        protected string _name;

        protected Pilot _pilot = new Pilot();
        protected Mech _mech = new Mech();
        protected Weapon _meleeWeapon = new Weapon();
        protected Weapon _rangedWeapon = new Weapon();

        protected ObservableStat _hitPoints = new ObservableStat(StatType.Health);

        protected readonly int _maxHitPoints = 10000;

        private Image _viewImage;
        private float _duration = 0.5f;
        private float _evasionMagnitude = 100f;
        private float _shakeStrength = 30f;
        private Vector3 _endScale = new Vector3(-0.25f, -0.25f, -0.25f);

        protected Combatant(string name)
        {
            _name = name;
        }

        public Pilot Pilot => _pilot;
        public Mech Mech => _mech;
        public Weapon MeleeWeapon => _meleeWeapon;
        public Weapon RangedWeapon => _rangedWeapon;
        public ObservableStat HitPoints => _hitPoints;
        public string Name => _name;

        public void SetUp(Image viewImage)
        {
            _viewImage = viewImage;
            _mech.GenerateStats();
            _meleeWeapon.GenerateStats();
            _rangedWeapon.GenerateStats();
            _pilot.SetStatsToDefault();
            _hitPoints.StatValue = _maxHitPoints;
        }

        public abstract void ChangePilotAbilityValue(bool doIncrease, StatType statType);

        public void GainMoraleForDodge(bool isSuccessful)
        {
            if (isSuccessful)
            {
                _pilot.GainMorale(AttackResult.DodgeSuccess);
                PlayEvadeAnimation();
            }
            else
                _pilot.GainMorale(AttackResult.DodgeFail);
        }

        public void GainMoraleForAttack(bool isSuccessful)
        {
            if (isSuccessful)
                _pilot.GainMorale(AttackResult.AttackSuccess);
            else
                _pilot.GainMorale(AttackResult.AttackFail);
        }

        public void GetDamage(int value, bool isDefending)
        {
            _hitPoints.StatValue -= value;

                PlayDamageAnimation(isDefending);
        }

        public void PlayDamageAnimation(bool isDefending)
        {
            if (_hitPoints.StatValue <= 0)
            {
                _viewImage.transform.DOShakePosition(_duration * 2, _shakeStrength * 2);
                var sequence = DOTween.Sequence();

                sequence.Insert(0.0f, _viewImage.transform.DOScale(Vector3.one * 2, _duration * 2));
                sequence.OnComplete(() =>
                {
                    sequence = null;
                    _viewImage.gameObject.SetActive(false);
                });

                return;
            }


            if (isDefending)
                _viewImage.transform.DOShakePosition(_duration, _shakeStrength / 4);
            else
            {
                _viewImage.transform.DOShakePosition(_duration, _shakeStrength);
                _viewImage.transform.DOShakeRotation(_duration, _shakeStrength);
            }
        }

        public void PlayEvadeAnimation()
        {
            _viewImage.transform.DOPunchPosition(Random.insideUnitSphere * _evasionMagnitude, _duration, 0);
            _viewImage.transform.DOPunchScale(_endScale, _duration, 0);
        }
    }
}