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

        public void SetUp()
        {
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
                _pilot.GainMorale(AttackResult.DodgeSuccess);
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

        public void GetDamage(int value)
        {
            _hitPoints.StatValue -= value;
        }
    }
}