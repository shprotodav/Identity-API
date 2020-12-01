using System;

namespace TestTask.Identity.Common.Exceptions
{
    [Serializable]
    public class IdentityDBException : Exception
    {
        public IdentityDBException(string message)
            : base(message)
        {
        }
    }
}
