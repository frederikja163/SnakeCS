using System.Collections.Generic;

namespace Snake.Data
{
    public interface IRenderer<T>
    {
        void Initialize(in T data);
        void Render(in T data);
    }

    public interface ISimulator<T>
    {
        void Initialize(in ControlList controls, in T data);

        T Tick(in T data);
    }
}