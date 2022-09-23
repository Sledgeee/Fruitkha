using System;
using System.Net;

namespace Fruitkha.Core.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public override string Message { get; }

        public HttpException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
