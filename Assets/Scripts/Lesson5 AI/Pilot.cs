namespace AI
{
    public class Pilot : IPublisher, ISubscriber
    {
        readonly int _defaultMorale = 100;
        readonly int _minMorale = 50;
        readonly int _maxMorale = 300;

        readonly int _defaultAbilityPoints = 30;

        ObservableStat _melee = new ObservableStat(StatType.Melee);
        ObservableStat _ranged = new ObservableStat( StatType.Ranged);
        ObservableStat _accuracy = new ObservableStat(StatType.Accuracy);
        ObservableStat _evasion = new ObservableStat(StatType.Evasion);
        ObservableStat _defence = new ObservableStat(StatType.Defence);
        ObservableStat _skill = new ObservableStat(StatType.Skill);
        ObservableStat _morale = new ObservableStat(StatType.Morale);

        ObservableStat _abilityPoints = new ObservableStat(StatType.NonSpecified);

        MoraleGain _moraleGain = new MoraleGain();

        public Pilot()
        {
            _morale.Supscribe(this);
        }

        public void SupscribeToStats(ISubscriber subscriber)
        {
            _melee.Supscribe(subscriber);
            _ranged.Supscribe(subscriber);
            _accuracy.Supscribe(subscriber);
            _evasion.Supscribe(subscriber);
            _defence.Supscribe(subscriber);
            _skill.Supscribe(subscriber);
            _morale.Supscribe(subscriber);
        }

        public void SubscribeToAbilityPoints(ISubscriber subscriber)
        {
            _abilityPoints.Supscribe(subscriber);
        }

        public void SetStatsToDefault()
        {
            _morale.StatValue = _defaultMorale;
            _abilityPoints.StatValue = _defaultAbilityPoints;
        }

        public void OnStatUpdate(ObservableStat stat, StatType dataType)
        {
            if (_morale.StatValue < _minMorale)
                _morale.StatValue = _minMorale;

            if (_morale.StatValue > _maxMorale)
                _morale.StatValue = _maxMorale;
        }

        public void IncrementOrDecrementStatValue(bool doIncrement, StatType statType)
        {
            if (doIncrement && _abilityPoints.StatValue <= 0)
                return;

            if (!doIncrement && _abilityPoints.StatValue >= _defaultAbilityPoints)
                return;

            ObservableStat stat;

            switch (statType)
            {
                case StatType.Accuracy:
                    {
                        stat = _accuracy;
                        break;
                    }

                case StatType.Defence:
                    {
                        stat = _defence;
                        break;
                    }

                case StatType.Evasion:
                    {
                        stat = _evasion;
                        break;
                    }

                case StatType.Melee:
                    {
                        stat = _melee;
                        break;
                    }

                case StatType.Ranged:
                    {
                        stat = _ranged;
                        break;
                    }

                case StatType.Skill:
                    {
                        stat = _skill;
                        break;
                    }
                default:
                    return;
            }

            if (!doIncrement && stat.StatValue <= 0)
                return;

            if(doIncrement)
            {
                stat.StatValue++;
                _abilityPoints.StatValue--;
            }
            else
            {
                stat.StatValue--;
                _abilityPoints.StatValue++;
            }
        }

        public void GainMorale(AttackResult result)
        {
            switch (result)
            {
                case AttackResult.AttackFail:
                    {
                        _morale.StatValue += _moraleGain.AttackFailGain;
                        return; 
                    }
                case AttackResult.AttackSuccess:
                    {
                        _morale.StatValue += _moraleGain.AttackSuccessGain;
                        return;
                    }
                case AttackResult.DodgeFail:
                    {
                        _morale.StatValue += _moraleGain.DodgeFailGain;
                        return;
                    }
                case AttackResult.DodgeSuccess:
                    {
                        _morale.StatValue += _moraleGain.DodgeSuccessGain;
                        return;
                    }
            }
        }

        public int Melee => _melee.StatValue;
        public int Ranged => _ranged.StatValue;
        public int Accuracy => _accuracy.StatValue;
        public int Evasion => _evasion.StatValue;
        public int Defence => _defence.StatValue;
        public int Morale => _morale.StatValue;
        public int Skill => _skill.StatValue;
    }
}