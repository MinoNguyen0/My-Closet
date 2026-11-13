using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Filters
{
    // The "show everything" filter. Always says yes.
    public class NoFilter : IItemFilter
    {
        public string Description => "All";

        public bool Matches(ClothingItem item)
        {
            return true;
        }
    }
}
