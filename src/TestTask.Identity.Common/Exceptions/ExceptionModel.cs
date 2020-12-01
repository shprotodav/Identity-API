namespace TestTask.Identity.Common.Exceptions
{
    public class ExceptionModel
    {
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public ExceptionModel InnerException { get; set; }
    }
}
