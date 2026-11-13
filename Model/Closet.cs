using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Filters;

namespace ClosetOutfitBuilder.Model
{
    // Holds every clothing item, sorted into one list per category.
    // Also remembers, for each category, the browsing position and which filter is on.
    public class Closet
    {
        // Position meaning "haven't browsed this category yet".
        private const int NothingChosen = -1;

        private readonly Dictionary<Category, List<ClothingItem>> _itemsByCategory;
        private readonly Dictionary<Category, int> _currentIndexByCategory;
        private readonly Dictionary<Category, IItemFilter> _currentFilterByCategory;

        public Closet()
        {
            _itemsByCategory = new Dictionary<Category, List<ClothingItem>>();
            _currentIndexByCategory = new Dictionary<Category, int>();
            _currentFilterByCategory = new Dictionary<Category, IItemFilter>();

            // Every category starts with an empty list, nothing chosen, and no filter.
            foreach (Category category in Enum.GetValues<Category>())
            {
                _itemsByCategory[category] = new List<ClothingItem>();
                _currentIndexByCategory[category] = NothingChosen;
                _currentFilterByCategory[category] = new NoFilter();
            }
        }

        // The item knows its own category, so it goes straight into the right list.
        public void AddItem(ClothingItem item)
        {
            _itemsByCategory[item.Category].Add(item);
        }

        // Returns the items in this category that the filter says yes to (read-only).
        public List<ClothingItem> GetByCategory(Category category, IItemFilter filter)
        {
            List<ClothingItem> result = new List<ClothingItem>();
            foreach (ClothingItem item in _itemsByCategory[category])
            {
                if (filter.Matches(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        // Turns a filter on for a category. The position is reset because the old one
        // might not exist in the shorter filtered list.
        public List<ClothingItem> ApplyFilter(Category category, IItemFilter filter)
        {
            _currentFilterByCategory[category] = filter;
            _currentIndexByCategory[category] = NothingChosen;
            return GetByCategory(category, filter);
        }

        // The filter currently on for a category (NoFilter if none).
        public IItemFilter CurrentFilter(Category category)
        {
            return _currentFilterByCategory[category];
        }

        // Right arrow. Returns null if there is nothing to show.
        public ClothingItem? CycleNext(Category category)
        {
            return Cycle(category, 1);
        }

        // Left arrow. Returns null if there is nothing to show.
        public ClothingItem? CyclePrevious(Category category)
        {
            return Cycle(category, -1);
        }

        // Moves forward (step = 1) or back (step = -1) through the filtered list,
        // wrapping around at either end: last -> first, first -> last.
        private ClothingItem? Cycle(Category category, int step)
        {
            List<ClothingItem> items = GetByCategory(category, _currentFilterByCategory[category]);
            if (items.Count == 0)
            {
                return null;
            }

            int current = _currentIndexByCategory[category];
            int index;
            if (current == NothingChosen || current >= items.Count)
            {
                // First press: Right starts at the first item, Left at the last.
                index = step > 0 ? 0 : items.Count - 1;
            }
            else
            {
                // % keeps the index between 0 and Count - 1 so it wraps round;
                // "+ items.Count" stops it going negative when stepping back from 0.
                index = (current + step + items.Count) % items.Count;
            }

            _currentIndexByCategory[category] = index;
            return items[index];
        }
    }
}
