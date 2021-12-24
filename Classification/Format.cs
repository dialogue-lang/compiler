using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Format
    {
        public string Type { get; set; }
        public int Start { get; set; }
        public int End { get; set; }

        public Format()
        {
            Type = "*";
            Start = 0;
            End = 0;
        }

        public Format(string type, int start, int end)
        {
            Type = type;
            Start = start;
            End = end;
        }

        public override int GetHashCode()
        {
            return Start ^ End;
        }
    }
}
