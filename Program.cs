using ClosetOutfitBuilder.Loading;
using ClosetOutfitBuilder.Model;
using ClosetOutfitBuilder.Storage;

namespace ClosetOutfitBuilder
{
    public class Program
    {
        public static void Main()
        {
            // Build the closet from the text file (the Factory creates each item).
            ClosetLoader loader = new ClosetLoader(new ClothingItemFactory());
            Closet closet = loader.Load("Data/closet.txt");

            // Load the last saved look (or an empty outfit the first time), then start.
            IOutfitStorage storage = new FileOutfitStorage("Data/outfit.txt");
            Outfit outfit = storage.Load(closet);

            ClosetApp app = new ClosetApp(closet, outfit, storage);
            app.Run();
        }
    }
}
