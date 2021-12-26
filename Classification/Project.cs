using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using Dialang.Compilation.Exceptions;
using Dialang.Compilation.IO;

namespace Dialang.Compilation.Classification
{
    public sealed class Project
    {
        public const string Extension = ".dlgproj";
        public const string FileSearch = "*.dlgscr";
        public const string CompiledExtension = ".dlg";

        private LoggingHandle log;
        private byte[] cache;
        private double elapsed;

        public string Path { get; }
        public string Name { get; }
        public IReadOnlyCollection<Document> Documents { get; }
        public double Elapsed => elapsed;

        public byte[] Compile()
        {
            log("Beginning project compilation...");
            if (cache != null)
            {
                log("Previous build was found, returning cache...");
                return cache;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using MemoryStream mem = new MemoryStream();
            using EntryWriter e = new EntryWriter(mem);

            e.Write(Name);
            e.Write(Documents.Count);

            foreach (Document doc in Documents)
            {
                e.Write(doc);
            }

            sw.Stop();
            elapsed += sw.Elapsed.TotalSeconds;
            cache = mem.ToArray();
            log($"Finished compiling into {cache.Length / 1000d:0.00}KB in {sw.Elapsed.TotalSeconds:0.00}s");

            return cache;
        }

        internal Project(string path, string name, LoggingHandle log)
        {
            if (!Directory.Exists(path) || File.Exists(System.IO.Path.Combine(path, name)))
                throw new InvalidProjectException(path, name);

            Path = path;
            Name = name;

            List<Document> docs = new List<Document>();
            DirectoryInfo dir = new DirectoryInfo(path);
            Document buf;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (var doc in dir.EnumerateFiles(FileSearch))
            {
                buf = new Document(doc, log);

                if (buf.Valid)
                {
                    log($"Loaded '{buf.Name}'!");
                    log("");
                    docs.Add(buf);
                }
            }

            Documents = docs;

            sw.Stop();
            elapsed = sw.Elapsed.TotalSeconds;
            log($"Finished loading in '{elapsed:0.00} seconds.'");

            this.log = log;
        }
    }
}
