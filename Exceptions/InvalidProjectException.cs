using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialang.Compilation.Exceptions
{
    public class InvalidProjectException : Exception
    {
        public InvalidProjectException(string path, string name) : base($"The project '{name}' does not exist at '{path}'.")
        {

        }
    }
}
