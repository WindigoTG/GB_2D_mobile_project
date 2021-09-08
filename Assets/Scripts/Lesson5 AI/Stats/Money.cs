namespace AI
{
    public class Money : PlayerStat
    {
        public Money(string statName = "")
            : base(statName == "" ? nameof(Money) : statName)
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
                    Notify(StatType.Money);
                }
            }
        }
    }
}