namespace ClosetOutfitBuilder.Harmony
{
    // One bold colour is a nice accent. Two bold colours only work if they match on the
    // colour wheel (close together, or opposite). Three or more usually clash.
    public class AccentRule : IColourRule
    {
        public double Score(List<ColourTone> tones)
        {
            List<ColourTone> bold = tones.Where(t => !t.IsNeutral).ToList();
            if (bold.Count <= 1)
            {
                return 1.0;
            }
            if (bold.Count > 2)
            {
                return 0.2;
            }

            double distance = bold[0].HueDistance(bold[1]);
            if (distance <= 45)
            {
                return 0.9;   // analogous: neighbours on the colour wheel
            }
            if (distance >= 150)
            {
                return 0.8;   // complementary: opposites on the colour wheel
            }
            return 0.3;
        }

        public string Praise(List<ColourTone> tones)
        {
            int bold = tones.Count(t => !t.IsNeutral);
            return bold switch
            {
                0 => "calm colours",
                1 => "one accent colour",
                _ => "matching accents"
            };
        }

        public string Tip => "too many bold colours";
    }
}
