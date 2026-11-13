using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // A skirt. Adds length and whether it's pleated.
    public class Skirt : ClothingItem
    {
        private readonly SkirtLength _skirtLength;
        private readonly bool _isPleated;

        public Skirt(string name, Color color, string imagePath, SkirtLength skirtLength, bool isPleated)
            : base(name, color, imagePath)
        {
            _skirtLength = skirtLength;
            _isPleated = isPleated;
        }

        public SkirtLength SkirtLength => _skirtLength;
        public bool IsPleated => _isPleated;

        // Same category as Pants: both go in the Bottoms slot of the outfit.
        public override Category Category => Category.Bottoms;

        // e.g. "Midi skirt, pleated"
        public override string Details => $"{_skirtLength} skirt" + (_isPleated ? ", pleated" : "");
    }
}
