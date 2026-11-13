using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Storage
{
    // Repository: the rest of the program saves and loads outfits through this interface,
    // without knowing whether they go to a text file, a database, or the cloud.
    public interface IOutfitStorage
    {
        void Save(Outfit outfit);

        // Needs the closet to turn saved item names back into real ClothingItem objects.
        Outfit Load(Closet closet);
    }
}
