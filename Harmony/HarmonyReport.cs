namespace ClosetOutfitBuilder.Harmony
{
    // The result of judging an outfit: a score out of 100 and a short reason.
    public class HarmonyReport
    {
        private readonly int _score;
        private readonly string _reason;

        public HarmonyReport(int score, string reason)
        {
            _score = score;
            _reason = reason;
        }

        public int Score => _score;
        public string Reason => _reason;

        public override string ToString() => $"Harmony {_score}/100: {_reason}";
    }
}
