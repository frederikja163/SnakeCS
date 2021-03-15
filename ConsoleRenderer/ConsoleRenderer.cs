using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Snake.Common;

namespace Snake.Rendering
{
    public sealed class ConsolePlatform : IPlatform<SnakeData>
    {
        public IReadOnlyCollection<IRenderer<SnakeData>> Renderers { get; } = new IRenderer<SnakeData>[]
        {
            new BackgroundRenderer(),
            new FruitRenderer(),
            new SnakeRenderer()
        };

        public IReadOnlyCollection<IUserInterface> UserInterfaces { get; } = new IUserInterface[]
        {
            new ConsoleUserInterface(),
        };
        
        public void Initialize(in ControlList controls, in SnakeData data)
        {
            foreach (var renderer in Renderers)
            {
                renderer.Initialize(data);
            }
            foreach (var userInterface in UserInterfaces)
            {
                userInterface.Initialize(controls);
            }
        }

        public void Tick(in SnakeData data)
        {
            foreach (var renderer in Renderers)
            {
                renderer.Render(data);
            }
            foreach (var userInterface in UserInterfaces)
            {
                userInterface.PollInput();
            }
        }
    }
    
    internal static class ConsoleUtility
    {
        public static void WriteToPosition(Vector2i position, char c)
        {
            if (position.X < 0 || position.X >= Console.BufferWidth ||
                position.Y < 0 || position.Y >= Console.BufferHeight)
                return;
            
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(c);
        }
        
        public static void WriteToPosition(Vector2i position, string str)
        {
            if (position.X < 0 || position.X >= Console.BufferWidth ||
                position.Y < 0 || position.Y >= Console.BufferHeight)
                return;
            
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(str);
        }
    }
    
    internal sealed class ConsoleUserInterface : IUserInterface
    {
        private readonly Dictionary<char, ControlCallback> _controls = new Dictionary<char, ControlCallback>();
        private char? _pressedKey;
        private readonly object _lockObj = new object();

        public void Initialize(in ControlList controls)
        {
            _controls.Add('w', controls[Control.TurnUp]);
            _controls.Add('a', controls[Control.TurnLeft]);
            _controls.Add('s', controls[Control.TurnDown]);
            _controls.Add('d', controls[Control.TurnRight]);
            _controls.Add('q', controls[Control.Close]);

            var thread = new Thread(InputThreadStart);
            thread.IsBackground = true;
            thread.Start();
        }

        public void PollInput()
        {
            lock (_lockObj)
            {
                if (_pressedKey == null)
                {
                    return;
                }

                if (_controls.TryGetValue(_pressedKey.Value, out var callback))
                {
                    callback.Invoke();
                }

                _pressedKey = null;
            }
        }

        private void InputThreadStart()
        {
            while (true)
            {
                char c = Console.ReadKey().KeyChar;
                lock (_lockObj)
                {
                    _pressedKey = c;
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }

    internal sealed class SnakeRenderer : IRenderer<SnakeData>
    {
        private Vector2i _lastTailPosition;

        public void Initialize(in SnakeData data)
        {
            SnakePart? current = data.SnakeBody.Tail;
            _lastTailPosition = current.Position;
            do
            {
                ConsoleUtility.WriteToPosition(current.Position, '¤');
            } while ((current = current.NextPartHeadDirection) != null);
        }

        public void Render(in SnakeData data)
        {
            SnakeBody body = data.SnakeBody;
            
            ConsoleUtility.WriteToPosition(_lastTailPosition, ' ');
            _lastTailPosition = body.Tail.Position;
            
            Vector2i headPosition = body.Head.Position;
            ConsoleUtility.WriteToPosition(headPosition, "¤");
            
            Console.SetCursorPosition(0, 0);
        }
    }

    internal sealed class FruitRenderer : IRenderer<SnakeData>
    {
        private Vector2i? _fruit;
        
        public void Initialize(in SnakeData data)
        {
        }

        public void Render(in SnakeData data)
        {
            if (_fruit != data.Fruit)
            {
                ConsoleUtility.WriteToPosition(data.Fruit, "@");
                _fruit = data.Fruit;
            }
        }
    }
    
    internal sealed class BackgroundRenderer : IRenderer<SnakeData>
    {
        public void Initialize(in SnakeData data)
        {
            int width = data.BoundingBox.Size.X;
            int height = data.BoundingBox.Size.Y;
            
            Console.SetCursorPosition(data.BoundingBox.Min.X - 1, data.BoundingBox.Min.Y - 1);
            char[] wallRow = new char[width + 3];
            char[] middleRow = new char[width + 3];
            for (int i = 0; i < wallRow.Length; i++)
            {
                wallRow[i] = '#';
                middleRow[i] = ' ';
            }
            middleRow[0] = '#';
            middleRow[^1] = '#';

            Console.WriteLine(wallRow);
            for (int i = data.BoundingBox.Min.Y; i <= data.BoundingBox.Max.Y; i++)
            {
                Console.SetCursorPosition(data.BoundingBox.Min.X - 1, i);
                Console.WriteLine(middleRow);
            }
            Console.WriteLine(wallRow);
            Console.SetCursorPosition(0, 0);
        }

        public void Render(in SnakeData data)
        {
            
        }
    }
}