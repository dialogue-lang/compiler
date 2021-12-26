using System;

namespace Dialang.Compilation
{
    public struct CompileResult
    {
        public string Message { get; }
        public Exception Exception { get; }
        public bool Success { get; }

        public override string ToString()
        {
            if (Exception == null)
                return Message;
            return Exception.ToString();
        }

        internal CompileResult(string msg, bool success = true)
        {
            Message = msg;
            Exception = null;
            Success = success;
        }

        internal CompileResult(Exception ex)
        {
            Message = ex.Message;
            Exception = ex;
            Success = false;
        }
    }
}