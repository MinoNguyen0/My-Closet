using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // A top. Inherits name, colour, picture and Display from ClothingItem; adds sleeve type and pattern.
    public class Shirt : ClothingItem
    {
        private readonly SleeveType _sleeveType;
        private readonly bool _hasPattern;

        public Shirt(string name, Color color, string imagePath, SleeveType sleeveType, bool hasPattern)
            : base(name, color, imagePath)
        {
            _sleeveType = sleeveType;
            _hasPattern = hasPattern;
        }

        public SleeveType SleeveType => _sleeveType;
        public bool HasPattern => _hasPattern;

        public override Category Category => Category.Shirt;

        // e.g. "Short sleeve, patterned". Sleeveless is written on its own (not "Sleeveless sleeve").
        public override string Details
        {
            get
            {
                string sleeve = _sleeveType switch
                {
                    SleeveType.Short => "Short sleeve",
                    SleeveType.Long => "Long sleeve",
                    _ => "Sleeveless"
                };
                return sleeve + (_hasPattern ? ", patterned" : ", plain");
            }
        }
    }
}
