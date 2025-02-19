using FizzBuzz.Appilcation.Dtos;
using FizzBuzz.Appilcation.Implementation;
using FizzBuzz.Appilcation.Interface;
using FizzBuzz.Domain.Exceptions;
using FizzBuzz.Domain.Implementation;
using FizzBuzz.Domain.Interface;
using FizzBuzz.Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Moq;

namespace FizzBuzz.Tests.Application
{
    public class FizzBuzzApplicationTests
    {
        private readonly Mock<IFizzBuzzService> _mockFizzBuzzService;
        private readonly Mock<IFileWriter> _mockFileWriter;
        private readonly Mock<ILogger<FizzBuzzApplication>> _mockLogger;
        private readonly FizzBuzzApplication _fizzBuzzApplication;

        public FizzBuzzApplicationTests()
        {
            _mockFizzBuzzService = new Mock<IFizzBuzzService>();
            _mockFileWriter = new Mock<IFileWriter>();
            _mockLogger = new Mock<ILogger<FizzBuzzApplication>>();

            _fizzBuzzApplication = new FizzBuzzApplication(
                _mockFizzBuzzService.Object,
                _mockLogger.Object,
                _mockFileWriter.Object
            );
        }

        [Fact]
        public async Task ReturnFizzBuzzList_WhenValidRequest()
        {
            /// Arrange
            var request = new FizzBuzzRequest { Start = 1, Limit = 5 };
            var expectedResult = new List<string> { "1", "2", "Fizz", "4", "Buzz" };

            // Mock the service to return the expected result
            _mockFizzBuzzService
                .Setup(s => s.GenerateFizzBuzzAsync(request.Start, request.Limit))
                .ReturnsAsync(expectedResult);

            // Mock the file writer to simulate no exception thrown (i.e., successful writing)
            _mockFileWriter.Setup(f => f.WriteToFile(It.IsAny<List<string>>()))
                           .Verifiable();

            // Act
            var result = await _fizzBuzzApplication.ProcessFizzBuzzAsync(request);

            // Assert
            // Ensure that the result matches the expected list
            Assert.Equal(expectedResult, result);

            // Verify that the service method was called exactly once
            _mockFizzBuzzService.Verify(s => s.GenerateFizzBuzzAsync(request.Start, request.Limit), Times.Once);

            // Verify that the file writer's method was called once
            _mockFileWriter.Verify(w => w.WriteToFile(expectedResult), Times.Once);
        }

        [Fact]
        public async Task ThrowGenerationException_WhenServiceFails()
        {
            // Arrange
            var request = new FizzBuzzRequest { Start = 1, Limit = 5 };

            _mockFizzBuzzService
                .Setup(s => s.GenerateFizzBuzzAsync(request.Start, request.Limit))
                .ThrowsAsync(new Exception("Service failure"));

            // Act & Assert
            await Assert.ThrowsAsync<FizzBuzzGenerationException>(
                () => _fizzBuzzApplication.ProcessFizzBuzzAsync(request)
            );

            _mockFizzBuzzService.Verify(s => s.GenerateFizzBuzzAsync(request.Start, request.Limit), Times.Once);
            _mockFileWriter.Verify(w => w.WriteToFile(It.IsAny<List<string>>()), Times.Never);
        }

        [Fact]
        public async Task ThrowException_WhenFileWriterFails()
        {
            // Arrange
            var request = new FizzBuzzRequest { Start = 1, Limit = 5 };
            var result = new List<string> { "1", "2", "Fizz", "4", "Buzz" };

            _mockFizzBuzzService
                .Setup(s => s.GenerateFizzBuzzAsync(request.Start, request.Limit))
                .ReturnsAsync(result);

            _mockFileWriter
                .Setup(w => w.WriteToFile(result))
                .Throws(new FileWriteException("File write error", new Exception("Inner exception details")));

            // Act & Assert
            await Assert.ThrowsAsync<FileWriteException>(
                () => _fizzBuzzApplication.ProcessFizzBuzzAsync(request)
            );

            _mockFizzBuzzService.Verify(s => s.GenerateFizzBuzzAsync(request.Start, request.Limit), Times.Once);
            _mockFileWriter.Verify(w => w.WriteToFile(result), Times.Once);
        }
    }
}