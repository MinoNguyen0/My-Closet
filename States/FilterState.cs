using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Filters;
using ClosetOutfitBuilder.Model;
using ClosetOutfitBuilder.UI;

namespace ClosetOutfitBuilder.States
{
    // Browsing one category with a filter on. Up / Down picks a different filter, 1 turns filtering off.
    // Left / Right browsing is inherited from CategoryState and only sees matching items.
    public class FilterState : CategoryState
    {
        private readonly Closet _closet;
        private readonly List<IItemFilter> _options;
        private int _optionIndex;

        public FilterState(Category category, Closet closet) : base(category)
        {
            _closet = closet;
            _options = BuildOptions(category);
            _optionIndex = 0;
            ApplyCurrentOption();
        }

        protected override void HandleModeInput(ClosetApp app)
        {
            if (KeyBindings.FilterTyped())
            {
                // Filter off: show everything again and go back to normal browsing.
                _closet.ApplyFilter(_category, new NoFilter());
                app.ChangeState(new BrowseState(_category));
            }
            else if (KeyBindings.DownTyped())
            {
                _optionIndex = (_optionIndex + 1) % _options.Count;
                ApplyCurrentOption();
            }
            else if (KeyBindings.UpTyped())
            {
                _optionIndex = (_optionIndex - 1 + _options.Count) % _options.Count;
                ApplyCurrentOption();
            }
        }

        protected override string ModeText(ClosetApp app)
        {
            return $"Filtering {_category}: {_options[_optionIndex].Description}   (up/down = change, 1 = off)";
        }

        private void ApplyCurrentOption()
        {
            _closet.ApplyFilter(_category, _options[_optionIndex]);
        }

        // The filters that make sense for each category (Strategy pattern: all are just IItemFilters).
        // Looping over the enums means a new shoe or accessory type gets a filter automatically.
        private static List<IItemFilter> BuildOptions(Category category)
        {
            List<IItemFilter> options = new List<IItemFilter>();

            switch (category)
            {
                case Category.Shirt:
                    foreach (SleeveType sleeve in Enum.GetValues<SleeveType>())
                    {
                        options.Add(new SleeveFilter(sleeve));
                    }
                    break;

                case Category.Bottoms:
                    options.Add(new TypeFilter<Pants>("Pants only"));
                    options.Add(new TypeFilter<Skirt>("Skirts only"));
                    break;

                case Category.Shoes:
                    foreach (ShoeType shoe in Enum.GetValues<ShoeType>())
                    {
                        options.Add(new ShoeTypeFilter(shoe));
                    }
                    break;

                case Category.Accessory:
                    foreach (AccessoryType accessory in Enum.GetValues<AccessoryType>())
                    {
                        options.Add(new AccessoryTypeFilter(accessory));
                    }
                    break;
            }

            return options;
        }
    }
}
