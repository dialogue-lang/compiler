using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Pause
    {
        public int Start { get; set; }
        public int Length { get; set; }

        public Pause()
        {
            Start = 0;
            Length = 0;
        }

        public Pause(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public override int GetHashCode()
        {
            return Start ^ Length;
        }
    }
}
