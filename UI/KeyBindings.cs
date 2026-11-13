using ClosetOutfitBuilder.Enums;
using SplashKitSDK;

namespace ClosetOutfitBuilder.UI
{
    // Every key the program uses, in one place.
    // To change a key, change it here and the whole program follows.
    public static class KeyBindings
    {
        // Returns the category whose key was just pressed (S, B, H, A), or null if none was.
        public static Category? CategoryTyped()
        {
            if (SplashKit.KeyTyped(KeyCode.SKey)) return Category.Shirt;
            if (SplashKit.KeyTyped(KeyCode.BKey)) return Category.Bottoms;
            if (SplashKit.KeyTyped(KeyCode.HKey)) return Category.Shoes;
            if (SplashKit.KeyTyped(KeyCode.AKey)) return Category.Accessory;
            return null;
        }

        public static bool NextTyped() => SplashKit.KeyTyped(KeyCode.RightKey);
        public static bool PreviousTyped() => SplashKit.KeyTyped(KeyCode.LeftKey);
        public static bool RandomizeTyped() => SplashKit.KeyTyped(KeyCode.RKey);
        public static bool SaveTyped() => SplashKit.KeyTyped(KeyCode.ReturnKey);
        public static bool BackTyped() => SplashKit.KeyTyped(KeyCode.EscapeKey);
        public static bool FilterTyped() => SplashKit.KeyTyped(KeyCode.Num1Key);
        public static bool MatchTyped() => SplashKit.KeyTyped(KeyCode.MKey);
        public static bool UpTyped() => SplashKit.KeyTyped(KeyCode.UpKey);
        public static bool DownTyped() => SplashKit.KeyTyped(KeyCode.DownKey);

        // Shown on the "controls" note so the user always knows what to press.
        public static readonly string[] HelpLines =
        {
            "S B H A    pick category",
            "<-  ->     browse",
            "1          filter on / off",
            "up  down   change filter",
            "R          surprise me",
            "M          match colours",
            "Enter      save look",
            "Esc        done"
        };
    }
}
