namespace ClosetOutfitBuilder.Harmony
{
    // Stylists repeat a colour somewhere else in the outfit (black shoes + black bag)
    // so it looks intentional. Scores how closely the two most similar pieces match.
    public class EchoRule : IColourRule
    {
        public double Score(List<ColourTone> tones)
        {
            double bestMatch = 0;
            for (int a = 0; a < tones.Count; a++)
            {
                for (int b = a + 1; b < tones.Count; b++)
                {
                    bestMatch = Math.Max(bestMatch, Similarity(tones[a], tones[b]));
                }
            }
            return bestMatch;
        }

        public string Praise(List<ColourTone> tones) => "repeated colour";

        public string Tip => "repeat one colour twice";

        // 1 = same colour, 0 = clearly different. Lightness matters most;
        // hue only matters when both colours actually have some colour in them.
        private static double Similarity(ColourTone x, ColourTone y)
        {
            double difference = Math.Abs(x.Lightness - y.Lightness) * 2
                              + Math.Abs(x.Saturation - y.Saturation);
            if (x.Saturation > 0.06 && y.Saturation > 0.06)
            {
                difference += x.HueDistance(y) / 180.0;
            }
            return Math.Max(0, 1 - difference / 0.3);
        }
    }
}
