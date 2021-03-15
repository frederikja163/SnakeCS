using System;
using System.Runtime.InteropServices;
using System.Threading;
using OpenTK.Mathematics;
using Snake.Common;
using Snake.Gl2DRenderer;
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
            ControlList controls = new ControlList();
            ISimulator<SnakeData>[] simulators = new ISimulator<SnakeData>[]
                {
                    new DirectionSimulator(controls, data),
                    new MoveSimulator(controls, data),
                    new FruitSimulator(controls, data),
                    new DeathSimulator(controls, data),
                };
            IPlatform<SnakeData>[] platforms = new IPlatform<SnakeData>[]
            {
                new ConsolePlatform(controls, data),
                new Gl2DPlatform(controls, data),
            };
            AppData<SnakeData> appData = new AppData<SnakeData>(data, simulators, platforms);
            
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
    }
}