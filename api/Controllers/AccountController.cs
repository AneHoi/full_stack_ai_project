using api.dtoModels;
using Microsoft.AspNetCore.Mvc;
using api.filter;
using Microsoft.AspNetCore.Mvc;
using service.accountservice;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _service;
    private readonly JwtService _jwtService;

    public AccountController(AccountService service, JwtService jwtService)
    {
        _service = service;
        _jwtService = jwtService;
    }

    [HttpPost]
    [Route("/account/login")]
    public ResponseDto Login([FromBody] LoginDto dto)
    {
        try
        {
            var user = _service.Authenticate(dto.email, dto.password);
            //Creating a token from the user
            //The "!" indicates that you are sure nullableString is not null
            var token = _jwtService.IssueToken(SessionData.FromUser(user!));
            return new ResponseDto
            {
                MessageToClient = "Successfully authenticated",
                ResponseData = new { token }
            };
        }
        catch (Exception e)
        {
            return new ResponseDto
            {
                MessageToClient = "Successfully authenticated",
                ResponseData = null
            };
        }
        
    }

    [HttpPost]
    [Route("/account/register")]
    public ResponseDto Register([FromBody] RegisterDto dto)
    {
        var user = _service.Register(dto.username, dto.tlfnumber, dto.email, dto.password);
        return new ResponseDto
        {
            MessageToClient = "Successfully registered",
            ResponseData = user
        };
    }

    [RequireAuthentication]
    [HttpPost]
    [Route("/account/Try")]
    public ResponseDto Register(string name)
    {
        Console.WriteLine("Hi Im: " + name);

        return new ResponseDto
        {
            MessageToClient = "Successfully registered",
            ResponseData = "You are now a user"
        };
    }

    [RequireAuthentication]
    [HttpGet]
    [Route("/account/whoami")]
    public ResponseDto WhoAmI()
    {
        //The only thing saved in the Session Data right now is the user id
        var data = HttpContext.GetSessionData();
        var user = _service.Get(data);
        return new ResponseDto
        {
            ResponseData = user
        };
    }
}