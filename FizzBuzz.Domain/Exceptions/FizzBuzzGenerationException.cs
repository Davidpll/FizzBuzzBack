

namespace FizzBuzz.Domain.Exceptions
{
    public class FizzBuzzGenerationException : Exception
    {
        public FizzBuzzGenerationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
