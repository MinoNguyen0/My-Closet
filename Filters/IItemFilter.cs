using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Filters
{
    // Strategy pattern: every filter answers one yes/no question about an item.
    // Closet doesn't know which filter it has; it just calls Matches().
    public interface IItemFilter
    {
        // Short text for the screen, e.g. "Short sleeve".
        string Description { get; }

        bool Matches(ClothingItem item);
    }
}
