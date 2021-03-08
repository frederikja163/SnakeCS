using System.Collections.Generic;

namespace Snake.Data
{
    public interface IRenderer
    {
        void Initialize(BoardData data);
        void Render(BoardData data);
    }

    public interface ISimulator
    {
        void Initialize(ControlList controls, BoardData data);

        BoardData Tick(BoardData data);
    }
}