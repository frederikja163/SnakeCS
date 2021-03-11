namespace Snake.Data
{
    public record AppData(
        BoardData Data,
        ISimulator[] Simulators, IRenderer[] Renderers, IUserInterface[] UserInterfaces);
}