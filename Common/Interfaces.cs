using System;

namespace Snake.Common
{
    public interface IPlatform<T>
    {
        void Tick(in T data);
    }

    public interface ISimulator<T>
    {
        T Tick(in T data);
    }
}