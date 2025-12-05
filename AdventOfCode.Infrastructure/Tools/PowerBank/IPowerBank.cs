using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Infrastructure.Tools.PowerBank
{
    public interface IPowerBank
    {
        public long JolatgeOutput(string bankInput, int VolatageCount);
    }
}
