using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizzBuzz.Domain.Interface
{
    public interface IFizzBuzzService
    {
        Task<List<string>> GenerateFizzBuzzAsync(int start, int limit);
    }
}
