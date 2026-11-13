using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Filters;
using ClosetOutfitBuilder.UI;

namespace ClosetOutfitBuilder.States
{
    // Browsing one category (the shared keys are in CategoryState). Adds: 1 = filter mode.
    public class BrowseState : CategoryState
    {
        public BrowseState(Category category) : base(category)
        {
        }

        protected override void HandleModeInput(ClosetApp app)
        {
            if (KeyBindings.FilterTyped())
            {
                app.ChangeState(new FilterState(_category, app.Closet));
            }
        }

        protected override string ModeText(ClosetApp app)
        {
            // A filter set earlier is remembered per category, so mention it if one is on.
            IItemFilter filter = app.Closet.CurrentFilter(_category);
            string filterText = filter is NoFilter ? "" : $"  (filter: {filter.Description})";
            return $"Browsing: {_category}{filterText}   (1 = filter)";
        }
    }
}
