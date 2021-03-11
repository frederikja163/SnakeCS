using System;
using System.Runtime.InteropServices;
using System.Threading;
using OpenTK.Mathematics;
using Snake.Data;
using Snake.Rendering;
using Snake.Simulation;

namespace Snake.EntryPoint
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Box2i boundingBox = new Box2i(1, 1, 25, 10);
            SnakeBody snake = CreateSnake(5, (Vector2i)boundingBox.Center);
            BoardData data = new BoardData(snake, Direction.Right, boundingBox, (Vector2i)boundingBox.Center + Vector2i.UnitX * 5);
            ControlList controls = new ControlList();

            AppData appData = CreateConsoleApp(data);
            
            foreach (var simulator in appData.Simulators)
            {
                simulator.Initialize(controls, data);
            }
            foreach (var renderer in appData.Renderers)
            {
                renderer.Initialize(data);
            }
            foreach (var userInterface in appData.UserInterfaces)
            {
                userInterface.Initialize(ref controls);
            }

            bool isRunning = true;
            controls[Control.Close] = () => isRunning = false;
            while (isRunning && data.IsAlive)
            {
                foreach (var userInterface in appData.UserInterfaces)
                {
                    userInterface.PollInput();
                }
                
                foreach (var simulator in appData.Simulators)
                {
                    data = simulator.Tick(data);
                }

                foreach (var renderer in appData.Renderers)
                {
                    renderer.Render(data);
                }
                Thread.Sleep(100);
            }
        }

        private static SnakeBody CreateSnake(int snakeLength, Vector2i headPosition)
        {
            SnakePart head = new SnakePart(headPosition);
            SnakePart current = head;
            for (int i = 0; i < snakeLength - 1; i++)
            {
                current = new SnakePart( current.Position - Vector2i.UnitX) {NextPartHeadDirection = current};
            }
            SnakePart tail = current;

            SnakeBody body = new SnakeBody(head, tail);
            return body;
        }

        private static AppData CreateConsoleApp(BoardData data)
        {
            return new AppData(data,
                new ISimulator[]
                {
                    new DirectionSimulator(),
                    new MoveSimulator(),
                    new FruitSimulator(),
                    new DeathSimulator(),
                },
                new IRenderer[]
                {
                    new BackgroundRenderer(),
                    new SnakeRenderer(),
                    new FruitRenderer(),
                },
                new IUserInterface[]
                {
                    new ConsoleUserInterface(),
                }
            );
        }
    }
}