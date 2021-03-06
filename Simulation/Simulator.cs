using System;
using OpenTK.Mathematics;
using Snake.Data;

namespace Snake.Simulation
{
    public sealed class Simulator : ISimulator
    {
        private readonly ISimulator[] _simulators = new ISimulator[] { new SnakeMoveSimulator() };
        
        public BoardData Tick(BoardData data)
        {
            foreach (var simulator in _simulators)
            {
                data = simulator.Tick(data);
            }
            return data;
        }
    }

    internal sealed class SnakeMoveSimulator : ISimulator
    {
        public BoardData Tick(BoardData data)
        {
            var oldBody = data.SnakeBody;
            var directionVector = data.Direction switch
            {
                Direction.None => Vector2i.Zero,
                Direction.Up => -Vector2i.UnitY,
                Direction.Right => Vector2i.UnitX,
                Direction.Down => Vector2i.UnitY,
                Direction.Left => -Vector2i.UnitX,
                _ => throw new ArgumentOutOfRangeException()
            };

            SnakePart oldHead = oldBody.Head;
            SnakePart newHead = oldHead with {Position = oldHead.Position + directionVector};
            oldHead.NextPartHeadDirection = newHead;

            SnakePart oldTail = oldBody.Tail;
            SnakePart newTail = oldTail.NextPartHeadDirection!;
            SnakeBody newBody = oldBody with {Head = newHead, Tail = newTail};

            return data with {SnakeBody = newBody};
        }
    }
}