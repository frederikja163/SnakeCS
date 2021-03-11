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
            SnakeData data = new SnakeData(snake, Direction.Right, boundingBox, (Vector2i)boundingBox.Center + Vector2i.UnitX * 5);

            AppData<SnakeData> appData = CreateConsoleApp(data, new ControlList());

            appData.Initialize();
            
            bool isRunning = true;
            appData.Controls[Control.Close] = () => isRunning = false;
            while (isRunning && appData.Data.IsAlive)
            {
                appData.Tick();
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

        private static AppData<SnakeData> Create2dOpentkApp(SnakeData data, ControlList controls)
        {
            return new AppData<SnakeData>(data,
                new ISimulator<SnakeData>[]
                {
                    new DirectionSimulator(),
                    new MoveSimulator(),
                    new FruitSimulator(),
                    new DeathSimulator(),
                },
                new IRenderer<SnakeData>[]
                {
                    
                },
                new IUserInterface[]
                {
                    
                }
            );
        }

        private static AppData<SnakeData> CreateConsoleApp(SnakeData data, ControlList controls)
        {
            return new AppData<SnakeData>(data,
                new ISimulator<SnakeData>[]
                {
                    new DirectionSimulator(),
                    new MoveSimulator(),
                    new FruitSimulator(),
                    new DeathSimulator(),
                },
                new IRenderer<SnakeData>[]
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