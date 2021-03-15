using System.Collections.Generic;

namespace Snake.Common
{
    public sealed class AppData<T>
    {
        public T Data { get; private set; }
        public ControlList Controls { get; }
        public IReadOnlyCollection<ISimulator<T>> Simulators { get; }
        public IReadOnlyCollection<IPlatform<T>> Platforms { get; }

        public AppData(T data, IReadOnlyCollection<ISimulator<T>> simulators, IReadOnlyCollection<IPlatform<T>> platforms, ControlList? controls = null)
        {
            Data = data;
            Simulators = simulators;
            Platforms = platforms;
            
            Controls = controls ?? new ControlList();
        }
        
        public void Initialize()
        {
            foreach (var simulator in Simulators)
            {
                simulator.Initialize(Controls, Data);
            }

            foreach (var platform in Platforms)
            {
                platform.Initialize(Controls, Data);
            }
        }

        public void Tick()
        {
            foreach (var simulator in Simulators)
            {
                Data = simulator.Tick(Data);
            }
            
            foreach (var platform in Platforms)
            {
                platform.Tick(Data);
            }
        }
    }
}