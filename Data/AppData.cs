using System.Collections.Generic;

namespace Snake.Data
{
    public class AppData<T>
    {
        public T Data { get; private set; }
        public readonly ControlList Controls;
        public readonly IReadOnlyCollection<ISimulator<T>> Simulators;
        public readonly IReadOnlyCollection<IRenderer<T>> Renderers;
        public readonly IReadOnlyCollection<IUserInterface> UserInterfaces;

        public AppData(T data, ISimulator<T>[] simulators, IRenderer<T>[] renderers, IUserInterface[] userInterfaces, ControlList? controls = null)
        {
            Data = data;
            Simulators = simulators;
            Renderers = renderers;
            UserInterfaces = userInterfaces;
            Controls = controls ?? new ControlList();
        }
        
        public void Initialize()
        {
            foreach (var simulator in Simulators)
            {
                simulator.Initialize(Controls, Data);
            }
            foreach (var renderer in Renderers)
            {
                renderer.Initialize(Data);
            }
            foreach (var userInterface in UserInterfaces)
            {
                userInterface.Initialize(Controls);
            }
        }

        public void Tick()
        {
            foreach (var userInterface in UserInterfaces)
            {
                userInterface.PollInput();
            }
                
            foreach (var simulator in Simulators)
            {
                Data = simulator.Tick(Data);
            }

            foreach (var renderer in Renderers)
            {
                renderer.Render(Data);
            }
        }
    }
}