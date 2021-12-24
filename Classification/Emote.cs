using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Emote
    {
        public string Name { get; set; }
        public int Position { get; set; }

        public Emote()
        {
            Name = "template_emote";
            Position = 0;
        }

        public Emote(string name, int position)
        {
            Name = name;
            Position = position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }
}
