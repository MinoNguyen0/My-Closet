using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Model
{
    // The outfit being worn: at most one item per category.
    public class Outfit
    {
        // Where each category is drawn (x, y, size), like a lookbook model:
        // top and bottoms in the middle, shoes below, accessory at the side.
        private static readonly Dictionary<Category, (double X, double Y, int Size)> Layout =
            new Dictionary<Category, (double X, double Y, int Size)>
            {
                { Category.Shirt,     (230, 95, 170) },
                { Category.Bottoms,   (235, 315, 180) },
                { Category.Shoes,     (275, 530, 100) },
                { Category.Accessory, (75, 300, 100) }
            };

        private readonly Dictionary<Category, ClothingItem> _slots;

        public Outfit()
        {
            _slots = new Dictionary<Category, ClothingItem>();
        }

        // Puts an item in its slot, replacing whatever was there.
        // Throws if the item doesn't belong in that slot (e.g. shoes in the shirt slot), so the bug isn't hidden.
        public void SetItem(Category category, ClothingItem item)
        {
            if (item.Category != category)
            {
                throw new ArgumentException($"A {item.Category} can't go in the {category} slot.");
            }
            _slots[category] = item;
        }

        // Returns null if nothing has been chosen for that category yet.
        public ClothingItem? GetItem(Category category)
        {
            return _slots.TryGetValue(category, out ClothingItem? item) ? item : null;
        }

        // Returns a copy, so other code can read the outfit but not change it directly.
        public Dictionary<Category, ClothingItem> GetSelections()
        {
            return new Dictionary<Category, ClothingItem>(_slots);
        }

        // Draws every chosen item in its place; empty slots show a faint outline.
        public void Display()
        {
            foreach (Category category in Enum.GetValues<Category>())
            {
                (double x, double y, int size) = Layout[category];

                if (_slots.TryGetValue(category, out ClothingItem? item))
                {
                    item.Display(x, y, size);
                }
                else
                {
                    SplashKit.DrawRectangle(Color.LightGray, x, y, size, size);
                    SplashKit.DrawText(category.ToString(), Color.Gray, x + 6, y + 6);
                }
            }
        }
    }
}
