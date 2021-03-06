using System;
using OpenTK.Mathematics;
using Snake.Data;

namespace Snake.Rendering
{
    public sealed class ConsoleRenderer : IRenderer
    {
        private Vector2i _lastTailPosition = Vector2i.Zero;
        
        public void Render(BoardData data)
        {
            var body = data.SnakeBody;
            
            _lastTailPosition = body.Tail.Position;
            WriteToPosition(_lastTailPosition, ' ');
            
            Vector2i headPosition = body.Head.Position;
            WriteToPosition(headPosition, '#');
            
            Console.SetCursorPosition(0, 0);
        }

        private void WriteToPosition(Vector2i position, char c)
        {
            if (position.X < 0 || position.X >= Console.BufferWidth ||
                position.Y < 0 || position.Y >= Console.BufferHeight)
                return;
            
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(c);
        }
    }
}