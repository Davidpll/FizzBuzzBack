using FizzBuzz.Appilcation.Dtos;
using FizzBuzz.Appilcation.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FizzBuzz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FizzBuzzController : ControllerBase
    {
        private readonly IFizzBuzzApplication _fizzBuzzApplication;

        public FizzBuzzController(IFizzBuzzApplication fizzBuzzApplication)
        {
            _fizzBuzzApplication = fizzBuzzApplication;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateFizzBuzz([FromQuery] FizzBuzzRequest request)
        {
            request.Validate();
            try
            {
                var result = await _fizzBuzzApplication.ProcessFizzBuzzAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
