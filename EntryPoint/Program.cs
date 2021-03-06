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

            IRenderer renderer = new ConsoleRenderer();
            ISimulator simulator = new Simulator();
            
            while (true)
            {
                data = simulator.Tick(data);
                renderer.Render(data);
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