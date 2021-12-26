using System.IO;

using Dialang.Compilation.Classification;
using Dialang.Compilation.Exceptions;

namespace Dialang.Compilation
{
    public delegate void LoggingHandle(string msg);
    public sealed class Compiler
    {
        public Project Project { get; }

        private string TrimEndingDirectorySeparator(string dir)
        {
            if (dir.EndsWith('\\'))
                return dir.Remove(dir.Length - 1);
            return dir;
        }

        public CompileResult Compile(string output)
        {
            try
            {
                string dir = Path.GetDirectoryName(output)!;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using FileStream fs = File.Create($"{TrimEndingDirectorySeparator(output)}\\{Project.Name}.dlg");
                byte[] bin = Project.Compile();
                fs.Write(bin, 0, bin.Length);
                return new CompileResult("Success.");
            } catch (System.Exception ex)
            {
                return new CompileResult(ex);
            }
        }

        public Compiler(string input, LoggingHandle log)
        {
            if (!File.Exists(input))
                throw new FileNotFoundException($"Could not find the project file at '{input}'.");

            string name = Path.GetFileNameWithoutExtension(input);
            string path = Path.GetDirectoryName(input)!;

            if (!input.EndsWith(Project.Extension))
                throw new InvalidProjectException(path, name);

            log("Loading project...");
            Project = new Project(path, name, log);
        }

        public static bool ValidInput(string input)
        {
            return File.Exists(input) && input.EndsWith(Project.Extension);
        }
    }
}