using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using Snake.Data;

namespace Snake.Simulation
{
    
    public sealed class DirectionSimulator : ISimulator
    {
        private Direction? _nextDirection;
        private Direction _currentDirection;
        public void Initialize(ControlList controls, BoardData data)
        {
            _currentDirection = data.Direction;
            controls[Control.TurnUp] = () =>
            {
                if (_currentDirection != Direction.Down) _nextDirection = Direction.Up;
            };
            controls[Control.TurnLeft] = () =>
            {
                if (_currentDirection != Direction.Right) _nextDirection = Direction.Left;
            };
            controls[Control.TurnDown] = () =>
            {
                if (_currentDirection != Direction.Up) _nextDirection = Direction.Down;
            };
            controls[Control.TurnRight] = () =>
            {
                if (_currentDirection != Direction.Left) _nextDirection = Direction.Right;
            };
        }

        public BoardData Tick(BoardData data)
        {
            _currentDirection = data.Direction;
            if (_nextDirection != null)
            {
                _currentDirection = _nextDirection.Value;
                _nextDirection = null;
                return data with {Direction = _currentDirection};
            }

            return data;
        }
    }
    
    public sealed class MoveSimulator : ISimulator
    {

        public void Initialize(ControlList controls, BoardData data)
        {
        }

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

    public sealed class DeathSimulator : ISimulator
    {
        public void Initialize(ControlList controls, BoardData data)
        {
            
        }

        public BoardData Tick(BoardData data)
        {
            SnakeBody body = data.SnakeBody;
            SnakePart head = body.Head;
            SnakePart current = body.Tail;
            do
            {
                if (head.Position == current.Position)
                {
                    return data with {IsAlive = false};
                }
            } while ((current = current!.NextPartHeadDirection!) != head);

            return data;
        }
    }
    
    public sealed class FruitSimulator : ISimulator
    {
        private SnakePart _oldTail;
        
        public void Initialize(ControlList controls, BoardData data)
        {
            _oldTail = data.SnakeBody.Tail;
        }

        public BoardData Tick(BoardData data)
        {
            SnakePart oldTail = _oldTail;
            _oldTail = data.SnakeBody.Tail;
            if (data.SnakeBody.Head.Position == data.Fruit)
            {
                return data with {SnakeBody = data.SnakeBody with {Tail = oldTail}};
            }
            
            return data;
        }
    }
}