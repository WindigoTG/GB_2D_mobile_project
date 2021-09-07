namespace AI
{
    public class Health : PlayerStat
    {
        public Health(string statName = "")
            : base(statName == "" ? nameof(Health) : statName)
        {
        }

        public override int StatValue
        {
            get => _statValue;
            set
            {
                if (_statValue != value)
                {
                    _statValue = value;
                    Notify(StatType.Health);
                }
            }
        }
    }
}