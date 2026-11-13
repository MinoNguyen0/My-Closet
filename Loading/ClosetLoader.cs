using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Loading
{
    // Reads the closet file and fills a Closet, using the factory to build each item.
    public class ClosetLoader
    {
        private const char Separator = '|';

        private readonly ClothingItemFactory _factory;

        public ClosetLoader(ClothingItemFactory factory)
        {
            _factory = factory;
        }

        // Blank lines and lines starting with # are ignored.
        // A line with a mistake is skipped with a message saying which line and why,
        // so one typo doesn't stop the whole program from starting.
        public Closet Load(string filePath)
        {
            Closet closet = new Closet();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Closet file '{filePath}' not found, starting with an empty closet.");
                return closet;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line == "" || line.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = line.Split(Separator);
                for (int p = 0; p < parts.Length; p++)
                {
                    parts[p] = parts[p].Trim();
                }

                try
                {
                    string type = parts[0];
                    string[] fields = parts[1..];    // everything after the type
                    closet.AddItem(_factory.Create(type, fields));
                }
                catch (FormatException error)
                {
                    Console.WriteLine($"{Path.GetFileName(filePath)} line {i + 1} skipped: {error.Message}");
                }
            }

            return closet;
        }
    }
}
