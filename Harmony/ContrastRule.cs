namespace ClosetOutfitBuilder.Harmony
{
    // Some difference between the lightest and darkest piece keeps an outfit from looking flat.
    // The score rises smoothly with the difference and is full once it reaches 0.35.
    public class ContrastRule : IColourRule
    {
        private const double IdealRange = 0.35;

        public double Score(List<ColourTone> tones)
        {
            double range = tones.Max(t => t.Lightness) - tones.Min(t => t.Lightness);
            return 0.3 + 0.7 * Math.Min(1.0, range / IdealRange);
        }

        public string Praise(List<ColourTone> tones) => "light/dark contrast";

        public string Tip => "add a lighter or darker piece";
    }
}
