using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // Base class for every piece of clothing (abstract: only its subclasses can be created).
    // Template Method pattern: Display() fixes the drawing steps (picture -> name -> details),
    // and each subclass supplies its own Details.
    public abstract class ClothingItem
    {
        // Default size of the square each picture is fitted into.
        public const int ImageSize = 150;

        private readonly string _name;
        private readonly Color _color;
        private readonly string _imagePath;
        private Bitmap? _bitmap;
        private bool _triedLoading;

        protected ClothingItem(string name, Color color, string imagePath)
        {
            _name = name;
            _color = color;
            _imagePath = imagePath;
        }

        public string Name => _name;
        public Color Color => _color;
        public string ImagePath => _imagePath;

        // Each subclass says which category it belongs to.
        public abstract Category Category { get; }

        // Each subclass describes itself, e.g. "Long sleeve, patterned".
        public abstract string Details { get; }

        // Draws the item fitted into a square of the given size (small on the outfit, 260 in the polaroid).
        // The steps are the same for every item; only Details differs (polymorphism).
        public void Display(double x, double y, int size = ImageSize)
        {
            Bitmap? picture = GetBitmap();
            if (picture != null)
            {
                DrawPicture(picture, x, y, size);
            }
            else
            {
                DrawPlaceholder(x, y, size);
            }

            SplashKit.DrawText(Name, Color.Black, x, y + size + 6);
            SplashKit.DrawText(Details, Color.Gray, x, y + size + 20);
        }

        // Loads the PNG only the first time it's needed, then reuses it every frame.
        // Reuses the bitmap if another item already loaded the same file. Returns null if the file is missing.
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

        // Shrinks the picture to fit the square without stretching it (the smaller of the two ratios).
        // SplashKit scales around the picture's centre, so the full-size picture is first positioned
        // with its centre on the square's centre. (double) avoids whole-number division giving 0.
        private static void DrawPicture(Bitmap picture, double x, double y, int size)
        {
            double scale = Math.Min((double)size / picture.Width, (double)size / picture.Height);
            double drawX = x + (size - picture.Width) / 2.0;
            double drawY = y + (size - picture.Height) / 2.0;
            SplashKit.DrawBitmap(picture, drawX, drawY, SplashKit.OptionScaleBmp(scale, scale));
        }

        // Shown when the PNG is missing, so the program still works.
        // "No image" is 64 x 8 pixels in SplashKit's default font, hence -32 and -4 to centre it.
        private void DrawPlaceholder(double x, double y, int size)
        {
            SplashKit.FillRectangle(_color, x, y, size, size);
            SplashKit.DrawRectangle(Color.Black, x, y, size, size);
            SplashKit.DrawText("No image", Color.Black, x + size / 2 - 32, y + size / 2 - 4);
        }
    }
}
