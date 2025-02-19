using FizzBuzz.Appilcation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizzBuzz.Appilcation.Interface
{
    public interface IFizzBuzzApplication
    {
        Task<List<string>> ProcessFizzBuzzAsync(FizzBuzzRequest request);
    }
}
