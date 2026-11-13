using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Filters;
using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Storage
{
    // Saves the outfit to a text file, one line per item: "Category|Item name", e.g.
    //   Shirt|Dot Halter
    //   Bottoms|Grey Skirt
    public class FileOutfitStorage : IOutfitStorage
    {
        private const char Separator = '|';

        private readonly string _filePath;

        public FileOutfitStorage(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(Outfit outfit)
        {
            // Make sure the folder exists, e.g. "Data/".
            string? folder = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using StreamWriter writer = new StreamWriter(_filePath);
            foreach (KeyValuePair<Category, ClothingItem> slot in outfit.GetSelections())
            {
                writer.WriteLine($"{slot.Key}{Separator}{slot.Value.Name}");
            }
        }

        // Returns an empty outfit if there is no save file yet.
        // Broken lines, or names not found in the closet, are skipped instead of crashing.
        public Outfit Load(Closet closet)
        {
            Outfit outfit = new Outfit();
            if (!File.Exists(_filePath))
            {
                return outfit;
            }

            foreach (string line in File.ReadAllLines(_filePath))
            {
                string[] parts = line.Split(Separator);
                if (parts.Length != 2 || !Enum.TryParse(parts[0], out Category category))
                {
                    continue;
                }

                foreach (ClothingItem item in closet.GetByCategory(category, new NoFilter()))
                {
                    if (item.Name == parts[1])
                    {
                        outfit.SetItem(category, item);
                        break;
                    }
                }
            }
            return outfit;
        }
    }
}
