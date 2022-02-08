using System;

namespace Dialang.Compilation.Classification
{
    public sealed class Entry
    {
        public string Name { get; set; }
        public Script[] Scripts { get; set; }

        public int Add()
        {
            Script[] l = Scripts;
            Array.Resize(ref l, l.Length + 1);
            Scripts = l;
            Scripts[l.Length - 1] = new Script();
            return l.Length - 1;
        }

        public int Add(string text)
        {
            Script[] l = Scripts;
            Array.Resize(ref l, l.Length + 1);
            Scripts = l;
            Scripts[l.Length - 1] = new Script(text);
            return l.Length - 1;
        }

        public Entry()
        {
            Name = "template_entry";
            Scripts = new Script[0];
        }

        public Entry(string name, params Script[] lines)
        {
            Name = name;
            Scripts = lines;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
