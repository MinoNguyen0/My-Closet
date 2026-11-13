namespace ClosetOutfitBuilder.Harmony
{
    // Warm tones (browns, reds, beige) and cool tones (navy, blue-greys) look best kept apart.
    // Pure greys, black and white have no undertone, so they go with either.
    // The more colourful the warm and cool pieces are, the more a mix of them clashes.
    public class UndertoneRule : IColourRule
    {
        public double Score(List<ColourTone> tones)
        {
            double warm = tones.Where(t => t.IsWarm).Sum(t => t.Saturation);
            double cool = tones.Where(t => t.IsCool).Sum(t => t.Saturation);
            if (warm == 0 || cool == 0)
            {
                return 1.0;
            }
            // 0 when one side is barely there, 1 when warm and cool are equally strong.
            double clash = Math.Min(warm, cool) / Math.Max(warm, cool);
            return 1.0 - 0.6 * clash;
        }

        public string Praise(List<ColourTone> tones)
        {
            if (tones.Any(t => t.IsWarm))
            {
                return "warm tones together";
            }
            if (tones.Any(t => t.IsCool))
            {
                return "cool tones together";
            }
            return "clean monochrome";
        }

        public string Tip => "warm and cool tones mixed";
    }
}
