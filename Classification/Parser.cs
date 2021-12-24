using System;
using System.IO;

namespace Dialang.Compilation.Classification
{
    public sealed class Parser : IDisposable
    {
        private StringReader r;

        public void Dispose()
        {
            r.Dispose();
        }

        internal Parser(string text)
        {
            r = new StringReader(text);
        }
    }
}
