using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Filters
{
    // Yes only for shoes of the chosen type.
    public class ShoeTypeFilter : IItemFilter
    {
        private readonly ShoeType _shoeType;

        public ShoeTypeFilter(ShoeType shoeType)
        {
            _shoeType = shoeType;
        }

        public string Description => _shoeType.ToString();

        public bool Matches(ClothingItem item)
        {
            return item is Shoes shoes && shoes.ShoeType == _shoeType;
        }
    }
}
