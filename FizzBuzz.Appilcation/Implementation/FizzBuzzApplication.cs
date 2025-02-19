using FizzBuzz.Appilcation.Dtos;
using FizzBuzz.Appilcation.Interface;
using FizzBuzz.Domain.Exceptions;
using FizzBuzz.Domain.Interface;
using FizzBuzz.Infrastructure.Interface;
using Microsoft.Extensions.Logging;

namespace FizzBuzz.Appilcation.Implementation
{
    public class FizzBuzzApplication : IFizzBuzzApplication
    {
        private readonly IFizzBuzzService _fizzBuzzService;
        private readonly ILogger<FizzBuzzApplication> _logger;
        private readonly IFileWriter _fileWriter;

        public FizzBuzzApplication(IFizzBuzzService fizzBuzzService, ILogger<FizzBuzzApplication> logger, IFileWriter fileWriter)
        {
            _fizzBuzzService = fizzBuzzService;
            _logger = logger;
            _fileWriter = fileWriter;
        }

        public async Task<List<string>> ProcessFizzBuzzAsync(FizzBuzzRequest request)
        {
            try
            {
                _logger.LogInformation("Processing FizzBuzz from {Start} to {Limit}", request.Start, request.Limit);
                var result = await _fizzBuzzService.GenerateFizzBuzzAsync(request.Start, request.Limit);
                _logger.LogInformation("Processed {Count} FizzBuzz entries.", result.Count);
                _fileWriter.WriteToFile(result);
                return result;
            }
            catch (FileWriteException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing FizzBuzz.");
                throw new FizzBuzzGenerationException("Error processing FizzBuzz.", ex);
            }
        }
    }
}
