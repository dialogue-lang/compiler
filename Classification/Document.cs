using System.IO;

namespace Dialang.Compilation.Classification
{
    public class Document
    {
        public bool Valid { get; }
        public string Text { get; }

        internal Document(FileInfo info)
        {
            Text = File.ReadAllText(info.FullName);
        }
    }
}
