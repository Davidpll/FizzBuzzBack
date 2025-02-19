using FizzBuzz.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace FizzBuzz.Appilcation.Dtos
{
    public class FizzBuzzRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Start must be greater than 0.")]
        public int Start { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Limit must be greater than 0.")]
        public int Limit { get; set; }

        public void Validate()
        {
            if (Limit < Start)
            {
                throw new InvalidRangeException("Limit must be greater than or equal to Start.");
            }
        }
    }
}
