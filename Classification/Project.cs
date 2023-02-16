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

        private byte[] cache;
        private double elapsed;

        public string Path { get; }
        public string Name { get; }
        public IReadOnlyCollection<Document> Documents { get; }
        public int Count { get; }
        public double Elapsed => elapsed;

        public byte[] GetByteCode(LoggingHandle log)
        {
            if (cache != null)
            {
                log("Previous build was found, returning cache...");
                return cache;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using MemoryStream mem = new MemoryStream();
            Write(mem, log);
            cache = mem.ToArray();

            return cache;
        }

        public void Write(Stream output, LoggingHandle log, bool free = false)
        {
            log("Beginning project compilation...");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            using EntryWriter e = new EntryWriter(output);

            e.Write(Name);
            e.Write(Count);

            foreach (Document doc in Documents)
                e.Write(doc);
            sw.Stop();

            elapsed += sw.Elapsed.TotalSeconds;
            log($"Finished compiling in {sw.Elapsed.TotalSeconds:0.00}s");

            if (free)
                output.Dispose();
        }

        public Project(IEnumerable<Document> documents)
        {
            Documents = new List<Document>(documents);
        }

        public Project(params Document[] documents) : this((IEnumerable<Document>)documents) { }

        public Project(string path, string name, LoggingHandle log)
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
                    Count += buf.Entries.Count;
                    log($"Loaded '{buf.Name}'!");
                    log("");
                    docs.Add(buf);
                }
            }

            Documents = docs;

            sw.Stop();
            elapsed = sw.Elapsed.TotalSeconds;
            log($"Finished loading in '{elapsed:0.00} seconds.'");
        }
    }
}
