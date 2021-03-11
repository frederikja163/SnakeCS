using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Snake.Data
{
    public enum Direction
    {
        Invalid,
        None,
        Up,
        Right,
        Down,
        Left,
    }

    public record BoardData(SnakeBody SnakeBody, Direction Direction, Box2i BoundingBox, Vector2i Fruit, bool IsAlive = true);

    public record SnakeBody(SnakePart Head, SnakePart Tail) : IEnumerable<SnakePart>
    {
        public IEnumerator<SnakePart> GetEnumerator()
        {
            SnakePart current = Head;
            yield return current;
            while (current.NextPartHeadDirection != null)
            {
                current = current.NextPartHeadDirection;
                yield return current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public record SnakePart(Vector2i Position)
    {
        public SnakePart? NextPartHeadDirection { get; set; } = null;
    };
}