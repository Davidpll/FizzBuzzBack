

namespace FizzBuzz.Domain.Exceptions
{
    public class FileWriteException: Exception
    {
        public FileWriteException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
