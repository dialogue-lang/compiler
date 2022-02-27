using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Choice
    {
        public int Index { get; set; }
        public string Value { get; set; }

        public Choice()
        {
            Index = 0;
            Value = string.Empty;
        }

        public Choice(int index, string value)
        {
            Index = index;
            Value = value;
        }

        public override int GetHashCode()
        {
            return Index;
        }
    }
}
