

namespace FizzBuzz.Domain.Exceptions
{
    public class InvalidRangeException : Exception
    {
        public InvalidRangeException(string message) : base(message)
        {
        }
    }
}
