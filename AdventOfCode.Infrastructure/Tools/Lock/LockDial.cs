using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.Lock
{
    public sealed class LockDial : ILockDial
    {
        private const int TotalPositions = 100;

        private int _currentPosition;
        public int CurrentPosition => _currentPosition;

        public int PassedZeroCount { get; set; }

        public LockDial()
        {
            _currentPosition = 50;
            PassedZeroCount = 0;
        }
        public LockDial(int startingPosition)
        {
            if (startingPosition < 0 || startingPosition >= TotalPositions)
                throw new ArgumentOutOfRangeException(nameof(startingPosition), $"Starting position must be between 0 and {TotalPositions - 1}.");
            _currentPosition = startingPosition;


            PassedZeroCount = 0;
        }

        public void Turn(Direction dir, int steps)
        {
            if (dir == Direction.Right)
            {
                TurnClockwise(steps);
            }
            else if (dir == Direction.Left)
            {
                TurnCounterClockwise(steps);
            }
            else
            {
                throw new ArgumentException("Invalid direction.", nameof(dir));
            }
        }

        public void TurnClockwise(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps), "Steps must be non-negative.");

            int fullRotaions = steps / TotalPositions; //650 => 6

            if (_currentPosition + (steps % TotalPositions) >= TotalPositions)
            {
                fullRotaions++;
            }




            //if (_currentPosition == 0 && steps > 0 && fullRotaions > 0)
            //{
            //    fullRotaions--;
            //}

            PassedZeroCount += fullRotaions;

            _currentPosition = (_currentPosition + steps) % TotalPositions;

            //if (_currentPosition == 0 && steps > 0)
            //{
            //    PassedZeroCount++;
            //}



        }
        public void TurnCounterClockwise(int steps)
        {
            if (steps < 0)
                throw new ArgumentOutOfRangeException(nameof(steps), "Steps must be non-negative.");

            int fullRotaions = steps / TotalPositions; //650 => 6

            if (_currentPosition !=0 && _currentPosition - (steps % TotalPositions) <= 0)
            {
                fullRotaions++;
            }


            PassedZeroCount += fullRotaions;


            int pos = (_currentPosition - steps + TotalPositions) % TotalPositions;

            if (pos < 0)
            {
                pos += TotalPositions;
            }
            _currentPosition = pos;


        }
    }
}
