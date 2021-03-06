namespace Snake.Data
{
    public interface IRenderer
    {
        void Render(BoardData data);
    }

    public interface ISimulator
    {
        BoardData Tick(BoardData data);
    }
}