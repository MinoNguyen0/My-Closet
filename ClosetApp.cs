using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Filters;
using ClosetOutfitBuilder.Harmony;
using ClosetOutfitBuilder.Model;
using ClosetOutfitBuilder.States;
using ClosetOutfitBuilder.Storage;
using ClosetOutfitBuilder.UI;
using SplashKitSDK;

namespace ClosetOutfitBuilder
{
    // The controller. Owns the closet, the outfit, the storage and the current state, and runs the game loop.
    // It never checks which mode it's in; the current state object handles keys and drawing (State pattern).
    public class ClosetApp
    {
        private readonly Closet _closet;
        private readonly Outfit _outfit;
        private readonly IOutfitStorage _storage;
        private readonly Random _random;
        private readonly CompositeRule _harmony;
        private readonly OutfitSuggester _suggester;
        private IAppState _state;
        private string _statusMessage;

        // Colour-matching suggestions (best first), which one is shown, and which kept item
        // they were made for; pressing M again moves to the next suggestion.
        private List<Dictionary<Category, ClothingItem>> _suggestions;
        private int _suggestionIndex;
        private ClothingItem? _suggestionKeptItem;

        public ClosetApp(Closet closet, Outfit outfit, IOutfitStorage storage)
        {
            _closet = closet;
            _outfit = outfit;
            _storage = storage;
            _random = new Random();
            _harmony = CompositeRule.Standard();
            _suggester = new OutfitSuggester(_harmony);
            _state = new IdleState();
            _statusMessage = "";
            _suggestions = new List<Dictionary<Category, ClothingItem>>();
            _suggestionIndex = 0;
            _suggestionKeptItem = null;
        }

        public Closet Closet => _closet;
        public Outfit Outfit => _outfit;

        // A short message at the bottom of the page, e.g. "Look saved at 14:03".
        public string StatusMessage => _statusMessage;

        // The game loop: every frame the current state handles keys and draws,
        // then the stickers are drawn on top.
        public void Run()
        {
            Window window = new Window("My Closet", PageRenderer.WindowWidth, PageRenderer.WindowHeight);

            while (!window.CloseRequested)
            {
                SplashKit.ProcessEvents();
                _state.HandleInput(this); // the current mode handles keys
                _state.Draw(this);  
                PageRenderer.DrawDecorations();
                SplashKit.RefreshScreen(60);
            }

            window.Close();
        }

        public void ChangeState(IAppState state)
        {
            _state = state;
        }

        // Leaving one category for another: save first, then browse the new one.
        public void SwitchCategory(Category category)
        {
            SaveOutfit();
            ChangeState(new BrowseState(category));
        }

        // Picks one random item from each category, ignoring any filter.
        // Empty categories are skipped so nothing crashes.
        public void RandomizeOutfit()
        {
            foreach (Category category in Enum.GetValues<Category>())
            {
                List<ClothingItem> items = _closet.GetByCategory(category, new NoFilter());
                if (items.Count > 0)
                {
                    int index = _random.Next(items.Count);
                    _outfit.SetItem(category, items[index]);
                }
            }
            _statusMessage = "Surprise look!";
        }

        public void SaveOutfit()
        {
            _storage.Save(_outfit);
            _statusMessage = $"Look saved at {DateTime.Now:HH:mm}";
        }

        // How well the current outfit's colours go together, or null if fewer than 2 pieces are on.
        public HarmonyReport? CurrentHarmony()
        {
            List<ClothingItem> worn = _outfit.GetSelections().Values.ToList();
            return worn.Count >= 2 ? _harmony.Evaluate(worn) : null;
        }

        // Puts on the best colour-matched outfit. keep = a category whose current item must stay
        // (e.g. the skirt you're browsing). Pressing again moves to the next-best suggestion.
        public void SuggestOutfit(Category? keep)
        {
            ClothingItem? kept = keep != null ? _outfit.GetItem(keep.Value) : null;

            if (kept != _suggestionKeptItem || _suggestions.Count == 0)
            {
                Dictionary<Category, ClothingItem> locked = new Dictionary<Category, ClothingItem>();
                if (kept != null)
                {
                    locked[kept.Category] = kept;
                }
                _suggestions = _suggester.Suggest(_closet, locked, 10);
                _suggestionIndex = 0;
                _suggestionKeptItem = kept;
            }
            else
            {
                _suggestionIndex = (_suggestionIndex + 1) % _suggestions.Count;
            }

            if (_suggestions.Count == 0)
            {
                _statusMessage = "Nothing to match yet";
                return;
            }

            foreach (KeyValuePair<Category, ClothingItem> slot in _suggestions[_suggestionIndex])
            {
                _outfit.SetItem(slot.Key, slot.Value);
            }

            string forItem = kept != null ? $" to go with {kept.Name}" : "";
            _statusMessage = $"Match {_suggestionIndex + 1} of {_suggestions.Count}{forItem}";
        }
    }
}
