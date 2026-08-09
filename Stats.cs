namespace HeadlessCore
{
    public readonly struct Stats
    {
        public static Stats Zero => default;

        public int Caliber { get; }
        public int Spirituality { get; }
        public int Tinkering { get; }
        public int Resonance { get; }
        public int Willpower { get; }

        public Stats(
            int caliber = 0,
            int spirituality = 0,
            int tinkering = 0,
            int resonance = 0,
            int willpower = 0)
        {
            Caliber = caliber;
            Spirituality = spirituality;
            Tinkering = tinkering;
            Resonance = resonance;
            Willpower = willpower;
        }

        public Stats Clamp(int min = 0, int max = 99)
        {
            return new Stats(
                Math.Clamp(Caliber, min, max),
                Math.Clamp(Spirituality, min, max),
                Math.Clamp(Tinkering, min, max),
                Math.Clamp(Resonance, min, max),
                Math.Clamp(Willpower, min, max)
            );
        }
        public static Stats operator +(Stats a, Stats b)
        {
            return new Stats(
                a.Caliber + b.Caliber,
                a.Spirituality + b.Spirituality,
                a.Tinkering + b.Tinkering,
                a.Resonance + b.Resonance,
                a.Willpower + b.Willpower
            );
        }
        public static Stats operator -(Stats a, Stats b)
        {
            return new Stats(
                a.Caliber - b.Caliber,
                a.Spirituality - b.Spirituality,
                a.Tinkering - b.Tinkering,
                a.Resonance - b.Resonance,
                a.Willpower - b.Willpower
            );
        }
    }
}