using System.IO;
using System.Collections;
using System.Collections.Generic;

using Dialang.Compilation.IO;

namespace Dialang.Compilation.Classification
{
    public class Document
    {
        public bool Valid { get; }
        public string Name { get; }
        public string Text { get; }
        public Dictionary<string, Entry> Entries { get; }

        public Document(string name, string content, LoggingHandle log)
        {
            Name = name;
            Text = content;
            log($"Parsing '{Name}'...");

            using (DocumentParser p = new DocumentParser(Text, log))
            {
                try
                {
                    Entries = p.Parse();
                    Valid = true;
                } catch
                {
                    log("Invalid document.");
                    Valid = false;
                }
            }
        }

        internal Document(FileInfo info, LoggingHandle log) : this(info.Name, File.ReadAllText(info.FullName), log)
        {
        }
    }
}
