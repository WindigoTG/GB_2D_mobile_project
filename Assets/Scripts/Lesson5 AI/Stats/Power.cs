namespace AI
{
    public class Power : PlayerStat
    {
        public Power(string statName = "")
            : base(statName == "" ? nameof(Power) : statName)
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
                    Notify(StatType.Power);
                }
            }
        }
    }
}