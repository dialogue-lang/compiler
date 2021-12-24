using System;

namespace Dialang.Compilation.Classification
{
    public sealed class Entry
    {
        public string Name { get; set; }
        public Line[] Lines { get; set; }

        public int Add()
        {
            Line[] l = Lines;
            Array.Resize(ref l, 1);
            Lines = l;
            Lines[l.Length - 1] = new Line();
            return l.Length - 1;
        }

        public int Add(string text)
        {
            Line[] l = Lines;
            Array.Resize(ref l, 1);
            Lines = l;
            Lines[l.Length - 1] = new Line(text);
            return l.Length - 1;
        }

        public Entry()
        {
            Name = "template_entry";
            Lines = new Line[0];
        }

        public Entry(string name, params Line[] lines)
        {
            Name = name;
            Lines = lines;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
