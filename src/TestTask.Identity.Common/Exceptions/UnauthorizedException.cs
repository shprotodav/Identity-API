using System;

namespace TestTask.Identity.Common.Exceptions
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message)
           : base(message)
        {
        }
    }
}
