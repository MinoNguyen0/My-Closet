namespace ClosetOutfitBuilder.States
{
    // State pattern: each mode of the program (idle, browsing, filtering) is its own class.
    // ClosetApp just asks the current state to handle the keys and draw,
    // so it never needs if-statements about which mode it's in.
    public interface IAppState
    {
        void HandleInput(ClosetApp app);
        void Draw(ClosetApp app);
    }
}
