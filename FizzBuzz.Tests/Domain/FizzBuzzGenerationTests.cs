using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FizzBuzz.Domain.Implementation;

namespace FizzBuzz.Tests.Domain
 {     
    public class FizzBuzzServiceTests
        {
            private readonly FizzBuzzService _service;

            public FizzBuzzServiceTests()
            {
                _service = new FizzBuzzService();
            }

            [Fact]
            public async Task ShouldReturn_CorrectFizzBuzzSequence()
            {
                // Arrange
                int start = 1;
                int limit = 15;

                // Act
                var result = await _service.GenerateFizzBuzzAsync(start, limit);

                // Assert
                var expected = new List<string>
            {
                "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz",
                "11", "Fizz", "13", "14", "FizzBuzz"
            };

                result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
            }

            [Fact]
            public async Task ShouldReturn_SingleValue_WhenRangeIsOneNumber()
            {
                // Arrange
                int start = 5;
                int limit = 5;

                // Act
                var result = await _service.GenerateFizzBuzzAsync(start, limit);

                // Assert
                result.Should().ContainSingle().Which.Should().Be("Buzz");
            }
      }
 }

