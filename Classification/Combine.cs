using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Combine
    {
        public int Start { get; set; }
        public int End { get; set; }

        public Combine()
        {
            Start = 0;
            End = 0;
        }

        public Combine(int start, int end)
        {
            Start = start;
            End = end;
        }

        public override int GetHashCode()
        {
            return Start ^ End;
        }
    }
}
