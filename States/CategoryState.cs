using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Model;
using ClosetOutfitBuilder.UI;

namespace ClosetOutfitBuilder.States
{
    // Shared behaviour for every state that is "inside" a category (Browse and Filter).
    // Handles the keys that work the same in both, then lets the subclass handle its own keys.
    public abstract class CategoryState : IAppState
    {
        protected readonly Category _category;

        protected CategoryState(Category category)
        {
            _category = category;
        }

        public Category Category => _category;

        public void HandleInput(ClosetApp app)
        {
            Category? picked = KeyBindings.CategoryTyped();
            if (picked != null && picked != _category)
            {
                // Leaving this category: SwitchCategory auto-saves first.
                app.SwitchCategory(picked.Value);
                return;
            }

            if (KeyBindings.BackTyped())
            {
                app.ChangeState(new IdleState());
                return;
            }

            if (KeyBindings.RandomizeTyped())
            {
                app.RandomizeOutfit();
                return;
            }

            if (KeyBindings.MatchTyped())
            {
                // Keep the item being browsed in this category and match everything else to it.
                app.SuggestOutfit(_category);
                return;
            }

            if (KeyBindings.NextTyped())
            {
                Wear(app, app.Closet.CycleNext(_category));
            }
            else if (KeyBindings.PreviousTyped())
            {
                Wear(app, app.Closet.CyclePrevious(_category));
            }

            // Anything else is up to the specific state.
            HandleModeInput(app);
        }

        public virtual void Draw(ClosetApp app)
        {
            PageRenderer.DrawPage(app, ModeText(app));
            app.Outfit.Display();

            // Work out "2 / 5": where the worn item sits in this category's (filtered) list.
            ClothingItem? worn = app.Outfit.GetItem(_category);
            List<ClothingItem> items = app.Closet.GetByCategory(_category, app.Closet.CurrentFilter(_category));
            int position = worn != null ? items.IndexOf(worn) : -1;
            string counter = position >= 0 ? $"{position + 1} / {items.Count}" : $"- / {items.Count}";

            PageRenderer.DrawPolaroid(worn, $"{_category.ToString().ToUpper()}    {counter}");
            if (worn == null)
            {
                PageRenderer.DrawPolaroidHint("press <- or -> to browse");
            }
        }

        // Each subclass handles the keys that only make sense in its mode.
        protected abstract void HandleModeInput(ClosetApp app);

        // The line under the title describing what's happening, e.g. "Browsing: Shirt".
        protected abstract string ModeText(ClosetApp app);

        // Puts the item on the outfit. item is null when the category is empty.
        private void Wear(ClosetApp app, ClothingItem? item)
        {
            if (item != null)
            {
                app.Outfit.SetItem(_category, item);
            }
        }
    }
}
