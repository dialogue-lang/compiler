using System;

namespace Dialang.Compilation
{
    public struct CompileResult
    {
        public string Message { get; }
        public Exception? Exception { get; }

        public override string ToString()
        {
            if (Exception == null)
                return Message;
            return Exception.ToString();
        }

        internal CompileResult(string msg)
        {
            Message = msg;
            Exception = null;
        }

        internal CompileResult(Exception ex)
        {
            Message = ex.Message;
            Exception = ex;
        }
    }
}