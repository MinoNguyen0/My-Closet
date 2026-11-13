namespace ClosetOutfitBuilder.Harmony
{
    // Strategy pattern: one style rule about how the colours in an outfit work together.
    // Composite pattern: CompositeRule is also an IColourRule, made of other rules.
    public interface IColourRule
    {
        // 0 = breaks the rule badly, 1 = follows it perfectly.
        double Score(List<ColourTone> tones);

        // Short compliment shown when the outfit does well on this rule, e.g. "neutral base".
        string Praise(List<ColourTone> tones);

        // Short advice shown when the outfit does badly on this rule.
        string Tip { get; }
    }
}
