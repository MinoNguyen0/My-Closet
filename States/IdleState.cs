using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.UI;

namespace ClosetOutfitBuilder.States
{
    // Start screen: no category chosen. The user can pick one, randomize, match colours, or save.
    public class IdleState : IAppState
    {
        public void HandleInput(ClosetApp app)
        {
            Category? picked = KeyBindings.CategoryTyped();
            if (picked != null)
            {
                // Nothing to auto-save when leaving idle, so change state directly.
                app.ChangeState(new BrowseState(picked.Value));
            }
            else if (KeyBindings.RandomizeTyped())
            {
                app.RandomizeOutfit();
            }
            else if (KeyBindings.MatchTyped())
            {
                // Nothing is being browsed, so nothing has to be kept: best outfit overall.
                app.SuggestOutfit(null);
            }
            else if (KeyBindings.SaveTyped())
            {
                app.SaveOutfit();
            }
        }

        public void Draw(ClosetApp app)
        {
            PageRenderer.DrawPage(app, "Pick a category to start browsing");
            app.Outfit.Display();
            PageRenderer.DrawPolaroid(null, "no category picked");
            PageRenderer.DrawPolaroidHint("press S, B, H or A");
        }
    }
}
