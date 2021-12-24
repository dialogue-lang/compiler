using System.IO;
using System.Collections.Generic;

using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation.Classification
{
    public sealed class Project
    {
        public string Path { get; }
        public string Name { get; }
        public IReadOnlyCollection<Document> Documents { get; }

        internal Project(string path, string name)
        {
            if (!Directory.Exists(path) || File.Exists(System.IO.Path.Combine(path, name)))
                throw new InvalidProjectException(path, name);

            Path = path;
            Name = name;

            List<Document> docs = new List<Document>();
            DirectoryInfo dir = new DirectoryInfo(path);
            Document buf;

            foreach (var doc in dir.EnumerateFiles("*.dlg"))
            {
                buf = new Document(doc);

                if (buf.Valid)
                    docs.Add(buf);
            }

            Documents = docs;
        }
    }
}
