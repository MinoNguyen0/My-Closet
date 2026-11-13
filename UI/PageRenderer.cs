using ClosetOutfitBuilder.Harmony;
using ClosetOutfitBuilder.Model;
using SplashKitSDK;

namespace ClosetOutfitBuilder.UI
{
    // Draws the parts of the screen that look the same in every state:
    // the background, the paper sheet, the polaroid, the controls note and the stickers.
    // All the scrapbook styling lives here.
    public static class PageRenderer
    {
        public const int WindowWidth = 1000;
        public const int WindowHeight = 720;

        private static readonly Color Background = SplashKit.RGBColor(207, 226, 234);
        private static readonly Color Paper = SplashKit.RGBColor(251, 251, 248);
        private static readonly Color Ink = SplashKit.RGBColor(44, 44, 42);
        private static readonly Color Faint = SplashKit.RGBColor(136, 135, 128);
        private static readonly Color PhotoArea = SplashKit.RGBColor(241, 239, 232);

        // Polaroid position and size.
        private const int PolaroidX = 650;
        private const int PolaroidY = 50;
        private const int PolaroidWidth = 300;
        private const int PolaroidHeight = 350;
        private const int PhotoSize = 260;

        private const string DecorFolder = "Resources/images/decor/";

        // Stickers and tape (picture, centre x, centre y, width, tilt), placed where they don't cover clothes or text.
        private static readonly List<Decoration> Decorations = new List<Decoration>
        {
            new Decoration(DecorFolder + "blue_dot_tape.png", 800, 52, 130, -6),       // holding the polaroid
            new Decoration(DecorFolder + "grey_dot_tape.png", 595, 685, 110, -35),     // paper, bottom-right corner
            new Decoration(DecorFolder + "hamster_sticker.png", 915, 640, 130, 8),     // peeking over the controls note
            new Decoration(DecorFolder + "pearl_star.png", 548, 120, 70, 12),          // paper, top-right
            new Decoration(DecorFolder + "blue_dot_heart.png", 165, 47, 32, -10),      // next to the title
            new Decoration(DecorFolder + "heart_clef.png", 930, 455, 34, 12),          // controls note corner
            new Decoration(DecorFolder + "black_dot_star.png", 620, 415, 44, -15),     // between paper and polaroid
            new Decoration(DecorFolder + "pearl_clef.png", 110, 580, 50, -10)          // paper, bottom-left
        };

        // Background, paper sheet, title, mode line, harmony score, status message and controls note.
        public static void DrawPage(ClosetApp app, string modeText)
        {
            SplashKit.ClearScreen(Background);
            SplashKit.FillRectangle(Paper, 40, 30, 560, 660);

            SplashKit.DrawText("MY CLOSET", Ink, 70, 50);
            SplashKit.DrawText(modeText, Faint, 70, 66);

            // Live colour-harmony score, just under the mode line (the bottom is taken by the shoes' label).
            HarmonyReport? harmony = app.CurrentHarmony();
            if (harmony != null)
            {
                SplashKit.DrawText(harmony.ToString(), Ink, 70, 82);
            }

            if (app.StatusMessage != "")
            {
                SplashKit.DrawText(app.StatusMessage, Faint, 70, 665);
            }

            DrawControls();
        }

        // The big polaroid on the right. item can be null (nothing to show yet).
        public static void DrawPolaroid(ClothingItem? item, string caption)
        {
            SplashKit.FillRectangle(Color.White, PolaroidX, PolaroidY, PolaroidWidth, PolaroidHeight);
            SplashKit.DrawRectangle(Faint, PolaroidX, PolaroidY, PolaroidWidth, PolaroidHeight);

            double photoX = PolaroidX + 20;
            double photoY = PolaroidY + 20;
            SplashKit.FillRectangle(PhotoArea, photoX, photoY, PhotoSize, PhotoSize);

            if (item != null)
            {
                item.Display(photoX, photoY, PhotoSize);
            }

            SplashKit.DrawText(caption, Ink, photoX, PolaroidY + PolaroidHeight - 18);
        }

        // Writes a message in the middle of the empty polaroid photo area.
        public static void DrawPolaroidHint(string hint)
        {
            SplashKit.DrawText(hint, Faint, PolaroidX + 40, PolaroidY + 20 + PhotoSize / 2);
        }

        // Drawn last each frame, so tape sits on top of the polaroid and the hamster over the controls note.
        public static void DrawDecorations()
        {
            foreach (Decoration decoration in Decorations)
            {
                decoration.Draw();
            }
        }

        private static void DrawControls()
        {
            const int x = 650;
            const int y = 430;
            SplashKit.FillRectangle(Color.White, x, y, 300, 220);
            SplashKit.DrawRectangle(Faint, x, y, 300, 220);
            SplashKit.DrawText("controls", Ink, x + 16, y + 16);

            for (int i = 0; i < KeyBindings.HelpLines.Length; i++)
            {
                SplashKit.DrawText(KeyBindings.HelpLines[i], Ink, x + 16, y + 44 + i * 20);
            }
        }
    }
}
