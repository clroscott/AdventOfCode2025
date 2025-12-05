using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.Lock
{
    public interface ILockDial
    {
        public int CurrentPosition { get; }
        public int PassedZeroCount { get; set; }

        public void Turn(Direction dir, int steps);

    }
}
