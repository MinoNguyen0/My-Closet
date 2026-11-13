using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // A pair of shoes. Adds heel height (cm, 0 = no heel) and shoe type.
    public class Shoes : ClothingItem
    {
        private readonly float _heelHeight;
        private readonly ShoeType _shoeType;

        public Shoes(string name, Color color, string imagePath, float heelHeight, ShoeType shoeType)
            : base(name, color, imagePath)
        {
            _heelHeight = heelHeight;
            _shoeType = shoeType;
        }

        public float HeelHeight => _heelHeight;
        public ShoeType ShoeType => _shoeType;

        public override Category Category => Category.Shoes;

        // Only mentions the heel when there is one: "Heels, 7.5 cm heel" or just "Sneakers".
        public override string Details =>
            _heelHeight > 0 ? $"{_shoeType}, {_heelHeight:0.#} cm heel" : _shoeType.ToString();
    }
}
