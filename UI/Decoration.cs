using SplashKitSDK;

namespace ClosetOutfitBuilder.UI
{
    // A sticker or piece of tape placed on the page, just for looks.
    public class Decoration
    {
        private readonly string _imagePath;
        private readonly double _centreX;
        private readonly double _centreY;
        private readonly double _width;
        private readonly double _angle;
        private Bitmap? _bitmap;
        private bool _triedLoading;

        // centreX/centreY = where the middle of the sticker goes, width = how wide it is drawn,
        // angle = tilt in degrees (positive = clockwise).
        public Decoration(string imagePath, double centreX, double centreY, double width, double angle = 0)
        {
            _imagePath = imagePath;
            _centreX = centreX;
            _centreY = centreY;
            _width = width;
            _angle = angle;
        }

        public void Draw()
        {
            Bitmap? picture = GetBitmap();
            if (picture == null)
            {
                return;
            }

            // SplashKit scales and rotates around the picture's centre,
            // so the unscaled picture is positioned with its centre on (centreX, centreY).
            double scale = _width / picture.Width;
            DrawingOptions options = SplashKit.OptionRotateBmp(_angle, SplashKit.OptionScaleBmp(scale, scale));
            SplashKit.DrawBitmap(picture, _centreX - picture.Width / 2.0, _centreY - picture.Height / 2.0, options);
        }

        // Loads the PNG the first time, then reuses it. A missing file just means no sticker.
        private Bitmap? GetBitmap()
        {
            if (!_triedLoading)
            {
                _triedLoading = true;
                if (File.Exists(_imagePath))
                {
                    _bitmap = SplashKit.HasBitmap(_imagePath)
                        ? SplashKit.BitmapNamed(_imagePath)
                        : SplashKit.LoadBitmap(_imagePath, _imagePath);
                }
            }
            return _bitmap;
        }
    }
}
