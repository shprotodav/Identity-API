using System;

namespace TestTask.Identity.Common.Exceptions
{
    [Serializable]
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message)
            : base(message)
        {
        }
    }
    
}
