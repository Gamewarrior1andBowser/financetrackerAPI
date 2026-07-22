using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok("You are authenticated ");
    }
}