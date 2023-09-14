using System;

namespace anilibria.Exceptions
{
    class ApiException : Exception
    {
        public ApiException(string message, int code) : base(message)
        {
            Code = code;
        }

        public int Code { get; }
    }
}
