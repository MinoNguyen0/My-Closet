using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Filters
{
    // Yes only for shirts with the chosen sleeve type.
    public class SleeveFilter : IItemFilter
    {
        private readonly SleeveType _sleeveType;

        public SleeveFilter(SleeveType sleeveType)
        {
            _sleeveType = sleeveType;
        }

        public string Description => _sleeveType == SleeveType.Sleeveless ? "Sleeveless" : $"{_sleeveType} sleeve";

        public bool Matches(ClothingItem item)
        {
            // "item is Shirt shirt" checks the type AND gives a Shirt variable to read the sleeves from.
            // Anything that isn't a shirt gets false.
            return item is Shirt shirt && shirt.SleeveType == _sleeveType;
        }
    }
}
