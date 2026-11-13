using SplashKitSDK;

namespace ClosetOutfitBuilder.Harmony
{
    // A colour described the way people talk about clothes:
    // hue (which colour, 0-360 degrees), saturation (how bold, 0-1), lightness (dark 0 - light 1).
    public class ColourTone
    {
        private readonly double _hue;
        private readonly double _saturation;
        private readonly double _lightness;

        public ColourTone(Color color)
        {
            // Standard RGB -> HSL conversion. SplashKit stores R, G, B as 0-1.
            double r = color.R, g = color.G, b = color.B;
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double range = max - min;

            _lightness = (max + min) / 2;
            _saturation = range == 0 ? 0 : range / (1 - Math.Abs(2 * _lightness - 1));

            if (range == 0)
            {
                _hue = 0;
            }
            else if (max == r)
            {
                _hue = 60 * (((g - b) / range + 6) % 6);
            }
            else if (max == g)
            {
                _hue = 60 * ((b - r) / range + 2);
            }
            else
            {
                _hue = 60 * ((r - g) / range + 4);
            }
        }

        public double Hue => _hue;
        public double Saturation => _saturation;
        public double Lightness => _lightness;

        // Black, white, greys, and very dark or muted colours (navy, brown, beige) go with anything.
        public bool IsNeutral => _saturation < 0.2 || _lightness < 0.14 || _lightness > 0.9;

        // Only colours with a little colour in them have an undertone; pure greys have none.
        public bool IsWarm => _saturation >= 0.06 && (_hue < 70 || _hue >= 330);
        public bool IsCool => _saturation >= 0.06 && _hue >= 150 && _hue < 290;

        // Shortest distance around the colour wheel, 0-180 degrees.
        public double HueDistance(ColourTone other)
        {
            double d = Math.Abs(_hue - other._hue) % 360;
            return d > 180 ? 360 - d : d;
        }
    }
}
