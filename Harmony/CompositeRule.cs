using ClosetOutfitBuilder.Model;

namespace ClosetOutfitBuilder.Harmony
{
    // Composite pattern: a rule made of other rules, each with a weight.
    // It is itself an IColourRule, so code using it can't tell one rule from many.
    public class CompositeRule : IColourRule
    {
        private readonly List<(IColourRule Rule, double Weight)> _rules;

        public CompositeRule()
        {
            _rules = new List<(IColourRule Rule, double Weight)>();
        }

        // The standard set of style rules used by the program.
        public static CompositeRule Standard()
        {
            CompositeRule rules = new CompositeRule();
            rules.Add(new NeutralBaseRule(), 0.25);
            rules.Add(new AccentRule(), 0.20);
            rules.Add(new ContrastRule(), 0.20);
            rules.Add(new UndertoneRule(), 0.20);
            rules.Add(new EchoRule(), 0.15);
            return rules;
        }

        public void Add(IColourRule rule, double weight)
        {
            _rules.Add((rule, weight));
        }

        // Weighted average of all the rules' scores.
        public double Score(List<ColourTone> tones)
        {
            double total = 0, weights = 0;
            foreach ((IColourRule rule, double weight) in _rules)
            {
                total += rule.Score(tones) * weight;
                weights += weight;
            }
            return weights == 0 ? 0 : total / weights;
        }

        // The compliments of the (up to two) rules this outfit does best on.
        // On a tie, the more specific (lower-weight) rule wins, so the reason is more interesting.
        public string Praise(List<ColourTone> tones)
        {
            IEnumerable<string> praises = _rules
                .Where(r => r.Rule.Score(tones) >= 0.8)
                .OrderByDescending(r => Math.Round(r.Rule.Score(tones), 2))
                .ThenBy(r => r.Weight)
                .Take(2)
                .Select(r => r.Rule.Praise(tones));
            return string.Join(", ", praises);
        }

        public string Tip => "try the M key for a better match";

        // The advice of the rule this outfit does worst on.
        public string WorstTip(List<ColourTone> tones)
        {
            return _rules.OrderBy(r => r.Rule.Score(tones)).First().Rule.Tip;
        }

        // Judges a set of clothes: score out of 100, plus praise (good outfit) or a tip (weak outfit).
        public HarmonyReport Evaluate(IEnumerable<ClothingItem> items)
        {
            List<ColourTone> tones = items.Select(i => new ColourTone(i.Color)).ToList();
            int score = (int)Math.Round(Score(tones) * 100);
            string reason = score >= 70 ? Praise(tones) : WorstTip(tones);
            return new HarmonyReport(score, reason);
        }
    }
}
