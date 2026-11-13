using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Model;
using SplashKitSDK;

namespace ClosetOutfitBuilder.Loading
{
    // Factory pattern: turns one line of the closet file into the right kind of ClothingItem.
    // This is the only place that knows which constructor to call for which type.
    public class ClothingItemFactory
    {
        // How many details each type needs after the type word (name, colour, picture + extras).
        private static readonly Dictionary<string, int> DetailsNeeded = new Dictionary<string, int>
        {
            { "Shirt", 5 }, { "Pants", 5 }, { "Skirt", 5 }, { "Shoes", 5 }, { "Accessory", 4 }
        };

        // type   = "Shirt", "Pants", "Skirt", "Shoes" or "Accessory"
        // fields = name, colour, picture, then the extra details for that type.
        // Throws FormatException if the line doesn't make sense (ClosetLoader skips that line).
        public ClothingItem Create(string type, string[] fields)
        {
            if (!DetailsNeeded.TryGetValue(type, out int needed))
            {
                throw new FormatException($"unknown clothing type '{type}' (use Shirt, Pants, Skirt, Shoes or Accessory)");
            }
            RequireFields(type, fields, needed);

            string name = fields[0];
            Color color = ParseColor(fields[1]);
            string imagePath = fields[2];

            switch (type)
            {
                case "Shirt":
                    return new Shirt(name, color, imagePath, ParseEnum<SleeveType>(fields[3]), ParseYesNo(fields[4]));
                case "Pants":
                    return new Pants(name, color, imagePath, ParseEnum<PantsLength>(fields[3]), ParseYesNo(fields[4]));
                case "Skirt":
                    return new Skirt(name, color, imagePath, ParseEnum<SkirtLength>(fields[3]), ParseYesNo(fields[4]));
                case "Shoes":
                    return new Shoes(name, color, imagePath, ParseNumber(fields[3]), ParseEnum<ShoeType>(fields[4]));
                case "Accessory":
                    return new Accessory(name, color, imagePath, ParseEnum<AccessoryType>(fields[3]));
                default:
                    throw new FormatException($"unknown clothing type '{type}'");
            }
        }

        private static void RequireFields(string type, string[] fields, int needed)
        {
            if (fields.Length < needed)
            {
                throw new FormatException($"a {type} needs {needed} details after the type, but this line has {fields.Length}");
            }
        }

        // "58,57,59" -> a SplashKit colour
        private static Color ParseColor(string text)
        {
            string[] parts = text.Split(',');
            if (parts.Length != 3
                || !int.TryParse(parts[0].Trim(), out int r)
                || !int.TryParse(parts[1].Trim(), out int g)
                || !int.TryParse(parts[2].Trim(), out int b))
            {
                throw new FormatException($"colour '{text}' should look like 58,57,59");
            }
            return SplashKit.RGBColor(r, g, b);
        }

        // "Short" -> SleeveType.Short (not case sensitive); works for any enum type T.
        private static T ParseEnum<T>(string text) where T : struct, Enum
        {
            if (!Enum.TryParse(text, true, out T value) || !Enum.IsDefined(value))
            {
                throw new FormatException($"'{text}' is not one of: {string.Join(", ", Enum.GetNames<T>())}");
            }
            return value;
        }

        private static bool ParseYesNo(string text)
        {
            switch (text.ToLower())
            {
                case "yes":
                case "true":
                    return true;
                case "no":
                case "false":
                    return false;
                default:
                    throw new FormatException($"'{text}' should be yes or no");
            }
        }

        // InvariantCulture: always read "." as the decimal point, whatever the computer's region setting.
        private static float ParseNumber(string text)
        {
            if (!float.TryParse(text, System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out float value))
            {
                throw new FormatException($"'{text}' should be a number, e.g. 7.5");
            }
            return value;
        }
    }
}
