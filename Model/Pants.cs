using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // Pants or shorts. Adds length and whether they're formal.
    public class Pants : ClothingItem
    {
        private readonly PantsLength _pantsLength;
        private readonly bool _isFormal;

        public Pants(string name, Color color, string imagePath, PantsLength pantsLength, bool isFormal)
            : base(name, color, imagePath)
        {
            _pantsLength = pantsLength;
            _isFormal = isFormal;
        }

        public PantsLength PantsLength => _pantsLength;
        public bool IsFormal => _isFormal;

        // Pants and skirts share the Bottoms slot of the outfit.
        public override Category Category => Category.Bottoms;

        // e.g. "Full length, casual"
        public override string Details
        {
            get
            {
                string length = _pantsLength switch
                {
                    PantsLength.Shorts => "Shorts",
                    PantsLength.Cropped => "Cropped",
                    _ => "Full length"
                };
                return length + (_isFormal ? ", formal" : ", casual");
            }
        }
    }
}
