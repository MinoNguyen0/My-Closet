using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // A bag, piece of jewellery, belt, hat or hair clip. Adds the accessory type.
    public class Accessory : ClothingItem
    {
        private readonly AccessoryType _accessoryType;

        public Accessory(string name, Color color, string imagePath, AccessoryType accessoryType)
            : base(name, color, imagePath)
        {
            _accessoryType = accessoryType;
        }

        public AccessoryType AccessoryType => _accessoryType;

        public override Category Category => Category.Accessory;

        // e.g. "Bag"
        public override string Details => _accessoryType.ToString();
    }
}
