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
            SnakeBody snake = CreateSnake(5);
            BoardData data = new BoardData(snake, Direction.Right);
            ControlList controls = new ControlList();

            IRenderer[] renderers = new IRenderer[]
            {
                new SnakeRenderer(),
            };
            ISimulator[] simulators = new ISimulator[]
            {
                new DirectionSimulator(),
                new MoveSimulator(),
            };
            IUserInterface[] userInterfaces = new IUserInterface[]
            {
                new ConsoleUserInterface(),
            };
            
            foreach (var simulator in simulators)
            {
                simulator.Initialize(controls, data);
            }
            foreach (var renderer in renderers)
            {
                renderer.Initialize(data);
            }
            foreach (var userInterface in userInterfaces)
            {
                userInterface.Initialize(ref controls);
            }

            bool isRunning = true;
            controls[Control.Close] = () => isRunning = false;
            while (isRunning)
            {
                foreach (var userInterface in userInterfaces)
                {
                    userInterface.PollInput();
                }
                
                foreach (var simulator in simulators)
                {
                    data = simulator.Tick(data);
                }

                foreach (var renderer in renderers)
                {
                    renderer.Render(data);
                }
                Thread.Sleep(100);
            }
        }

        private static SnakeBody CreateSnake(int snakeLength)
        {
            SnakePart head = new SnakePart(Vector2i.UnitX * snakeLength);
            SnakePart current = head;
            for (int i = 0; i < snakeLength - 1; i++)
            {
                current = new SnakePart(current.Position - Vector2i.UnitX) {NextPartHeadDirection = current};
            }
            SnakePart tail = current;

            SnakeBody body = new SnakeBody(head, tail, snakeLength);
            return body;
        }
    }
}