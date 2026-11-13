using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Filters
{
    // Yes only for accessories of the chosen type.
    public class AccessoryTypeFilter : IItemFilter
    {
        private readonly AccessoryType _accessoryType;

        public AccessoryTypeFilter(AccessoryType accessoryType)
        {
            _accessoryType = accessoryType;
        }

        public string Description => _accessoryType.ToString();

        public bool Matches(ClothingItem item)
        {
            return item is Accessory accessory && accessory.AccessoryType == _accessoryType;
        }
    }
}
