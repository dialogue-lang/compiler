using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Event
    {
        public string Name { get; set; }
        public int Position { get; set; }

        public Event()
        {
            Name = "template_event";
            Position = 0;
        }

        public Event(string name, int position)
        {
            Name = name;
            Position = position;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
