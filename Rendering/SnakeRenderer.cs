using System;
using System.Collections.Generic;
using System.Threading;
using OpenTK.Mathematics;
using Snake.Data;

namespace Snake.Rendering
{
    public static class ConsoleUtility
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
    
    public sealed class ConsoleUserInterface : IUserInterface
    {
        private readonly Dictionary<char, ControlCallback> _controls = new Dictionary<char, ControlCallback>();
        private char? _pressedKey;
        private readonly object _lockObj = new object();

        public void Initialize(ref ControlList controls)
        {
            _controls.Add('w', controls[Control.TurnUp]);
            _controls.Add('a', controls[Control.TurnLeft]);
            _controls.Add('s', controls[Control.TurnDown]);
            _controls.Add('d', controls[Control.TurnRight]);

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

    public sealed class SnakeRenderer : IRenderer
    {
        private Vector2i _lastTailPosition;

        public void Initialize(BoardData data)
        {
            SnakePart? current = data.SnakeBody.Tail;
            _lastTailPosition = current.Position;
            ConsoleUtility.WriteToPosition(current.Position, '#');
            while ((current = current.NextPartHeadDirection) != null)
            {
                ConsoleUtility.WriteToPosition(current.Position, '#');
            }
        }

        public void Render(BoardData data)
        {
            var body = data.SnakeBody;
            
            ConsoleUtility.WriteToPosition(_lastTailPosition, ' ');
            _lastTailPosition = body.Tail.Position;
            
            Vector2i headPosition = body.Head.Position;
            ConsoleUtility.WriteToPosition(headPosition, '#');
            
            Console.SetCursorPosition(0, 0);
        }
    }
}