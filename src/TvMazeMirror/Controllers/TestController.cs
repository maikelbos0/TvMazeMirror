using Microsoft.AspNetCore.Mvc;

namespace TvMazeMirror.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger) {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public string Get() {
        return "Test";
    }
}