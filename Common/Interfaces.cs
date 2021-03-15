namespace Snake.Common
{
    public interface IRenderer<T>
    {
        void Initialize(in T data);
        void Render(in T data);
    }
    
    public interface IUserInterface
    {
        void Initialize(in ControlList controls);

        void PollInput();
    }

    public interface IPlatform<T>
    {
        void Initialize(in ControlList controls, in T data);
        void Tick(in T data);
    }

    public interface ISimulator<T>
    {
        void Initialize(in ControlList controls, in T data);

        T Tick(in T data);
    }
}