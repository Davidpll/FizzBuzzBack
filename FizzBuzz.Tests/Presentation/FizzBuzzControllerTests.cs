
using Moq;
using FizzBuzz.Appilcation.Dtos;
using FizzBuzz.Appilcation.Interface;
using FizzBuzz.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

public class FizzBuzzControllerTests
{
    private readonly Mock<IFizzBuzzApplication> _mockFizzBuzzApplication;
    private readonly FizzBuzzController _controller;

    public FizzBuzzControllerTests()
    {
        _mockFizzBuzzApplication = new Mock<IFizzBuzzApplication>();
        _controller = new FizzBuzzController(_mockFizzBuzzApplication.Object);
    }

    [Fact]
    public async Task ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new FizzBuzzRequest { Start = 1, Limit = 5 };
        var expectedResult = new List<string> { "1", "2", "Fizz", "4", "Buzz" };

        _mockFizzBuzzApplication
            .Setup(app => app.ProcessFizzBuzzAsync(request))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GenerateFizzBuzz(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualList = Assert.IsType<List<string>>(okResult.Value);
        Assert.Equal(expectedResult, actualList);
    }

    [Fact]
    public async Task ExceptionThrown_ReturnsInternalServerError()
    {
        // Arrange
        var request = new FizzBuzzRequest { Start = 1, Limit = 5 };

        _mockFizzBuzzApplication
            .Setup(app => app.ProcessFizzBuzzAsync(request))
            .ThrowsAsync(new System.Exception("Some error"));

        // Act
        var result = await _controller.GenerateFizzBuzz(request);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An unexpected error occurred.", objectResult.Value);
    }
}