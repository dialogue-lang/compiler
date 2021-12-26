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
        public Hashtable Entries { get; }

        internal Document(FileInfo info, LoggingHandle log)
        {
            Name = info.Name;
            Text = File.ReadAllText(info.FullName);
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
    }
}
