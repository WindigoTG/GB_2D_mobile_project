using System;

namespace AI
{
    public class MoraleGain
    {
        public int AttackSuccessGain { get; private set; }
        public int AttackFailGain { get; private set; }
        public int DodgeSuccessGain { get; private set; }
        public int DodgeFailGain { get; private set; }

        private readonly int _maxNegativeGain = -5;
        private readonly int _maxPositiveGain = 10;

        public MoraleGain()
        {
            Random rng = new Random();

            AttackSuccessGain = rng.Next(0, _maxPositiveGain);
            AttackFailGain = rng.Next(_maxNegativeGain, _maxPositiveGain);
            DodgeSuccessGain = rng.Next(0, _maxPositiveGain);
            DodgeFailGain = rng.Next(_maxNegativeGain, _maxPositiveGain);
        }
    }
}