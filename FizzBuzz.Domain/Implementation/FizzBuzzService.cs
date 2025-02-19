using FizzBuzz.Domain.Interface;

namespace FizzBuzz.Domain.Implementation
{
    public class FizzBuzzService : IFizzBuzzService
    {
        public async Task<List<string>> GenerateFizzBuzzAsync(int start, int limit)
        {
            var tasks = Enumerable.Range(start, limit - start + 1)
                .AsParallel()
                .Select(n => Task.Run(() => (n % 3 == 0, n % 5 == 0) switch
                {
                    (true, true) => "FizzBuzz",
                    (true, false) => "Fizz",
                    (false, true) => "Buzz",
                    _ => n.ToString()
                }))
                .ToList();

            var result = await Task.WhenAll(tasks);
            return result.ToList();
        }
    }
}