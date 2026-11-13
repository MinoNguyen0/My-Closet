namespace ClosetOutfitBuilder.Harmony
{
    // Outfits look put-together when at least half the pieces are neutral.
    public class NeutralBaseRule : IColourRule
    {
        public double Score(List<ColourTone> tones)
        {
            double neutralShare = (double)tones.Count(t => t.IsNeutral) / tones.Count;
            return Math.Min(1.0, neutralShare * 2);
        }

        public string Praise(List<ColourTone> tones) => "neutral base";

        public string Tip => "swap a bold piece for a neutral one";
    }
}
