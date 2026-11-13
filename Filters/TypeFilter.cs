using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Filters
{
    // Yes only for one kind of clothing, e.g. TypeFilter<Skirt> = "Skirts only".
    // T is filled in when the filter is created, so this one generic class works for any clothing type.
    public class TypeFilter<T> : IItemFilter where T : ClothingItem
    {
        private readonly string _description;

        public TypeFilter(string description)
        {
            _description = description;
        }

        public string Description => _description;

        public bool Matches(ClothingItem item)
        {
            return item is T;
        }
    }
}
