using api.dtoModels;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    
    [HttpPost]
    [Route("/account/register")]
    public String Register([FromBody] RegisterDto dto)
    {
        Console.WriteLine("Hi Im: \t\t" + dto.username);
        return "welcome";
    }
}
