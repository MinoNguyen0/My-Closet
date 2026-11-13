using ClosetOutfitBuilder.Enums;
using ClosetOutfitBuilder.Filters;
using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Harmony
{
    // Finds the best-matching outfits by scoring every possible combination
    // (one item per category), then picks a varied set from the best ones.
    public class OutfitSuggester
    {
        // How many top-scoring outfits to consider before choosing a varied few from them.
        private const int CandidatePool = 200;

        private readonly IColourRule _rule;

        public OutfitSuggester(IColourRule rule)
        {
            _rule = rule;
        }

        // locked = items that must stay (e.g. "I'm wearing this skirt"); everything else is chosen.
        // Returns up to howMany outfits, best first, each clearly different from the ones before it.
        public List<Dictionary<Category, ClothingItem>> Suggest(Closet closet, Dictionary<Category, ClothingItem> locked, int howMany)
        {
            // The choices for each category: just the locked item, or everything in the closet.
            List<(Category Category, List<ClothingItem> Items)> choices = new List<(Category, List<ClothingItem>)>();
            foreach (Category category in Enum.GetValues<Category>())
            {
                List<ClothingItem> items = locked.TryGetValue(category, out ClothingItem? keep)
                    ? new List<ClothingItem> { keep }
                    : closet.GetByCategory(category, new NoFilter());
                if (items.Count > 0)
                {
                    choices.Add((category, items));
                }
            }

            // Work out each item's tone once, instead of once per combination.
            Dictionary<ClothingItem, ColourTone> tones = new Dictionary<ClothingItem, ColourTone>();
            foreach ((_, List<ClothingItem> items) in choices)
            {
                foreach (ClothingItem item in items)
                {
                    tones[item] = new ColourTone(item.Color);
                }
            }

            // A min-heap holding only the best CandidatePool outfits seen so far:
            // the worst of them sits on top and is thrown away when a better one arrives.
            PriorityQueue<ClothingItem[], double> best = new PriorityQueue<ClothingItem[], double>();
            ClothingItem[] current = new ClothingItem[choices.Count];
            List<ColourTone> currentTones = new List<ColourTone>(choices.Count);

            void TryAll(int depth)
            {
                if (depth == choices.Count)
                {
                    currentTones.Clear();
                    foreach (ClothingItem item in current)
                    {
                        currentTones.Add(tones[item]);
                    }
                    double score = _rule.Score(currentTones);
                    best.Enqueue((ClothingItem[])current.Clone(), score);
                    if (best.Count > CandidatePool)
                    {
                        best.Dequeue();
                    }
                    return;
                }
                foreach (ClothingItem item in choices[depth].Items)
                {
                    current[depth] = item;
                    TryAll(depth + 1);
                }
            }

            if (choices.Count > 0)
            {
                TryAll(0);
            }

            // The heap gives worst-first, so reverse to get best-first.
            List<ClothingItem[]> candidates = new List<ClothingItem[]>();
            while (best.Count > 0)
            {
                candidates.Add(best.Dequeue());
            }
            candidates.Reverse();

            // Go down the list best-first, keeping an outfit only if it shares at most one
            // (not locked) item with every outfit already kept, so suggestions feel different.
            List<ClothingItem[]> chosen = new List<ClothingItem[]>();
            foreach (ClothingItem[] candidate in candidates)
            {
                if (chosen.Count == howMany)
                {
                    break;
                }
                if (chosen.All(other => SharedItems(candidate, other, locked) <= 1))
                {
                    chosen.Add(candidate);
                }
            }

            return chosen.Select(outfit => outfit.ToDictionary(item => item.Category)).ToList();
        }

        private static int SharedItems(ClothingItem[] a, ClothingItem[] b, Dictionary<Category, ClothingItem> locked)
        {
            int shared = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i] && !locked.ContainsKey(a[i].Category))
                {
                    shared++;
                }
            }
            return shared;
        }
    }
}
