using api.dtoModels;
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

    [HttpPost]
    [Route("/account/register")]
    public ResponseDto Register([FromBody] RegisterDto dto)
    {
        Console.WriteLine("Hi Im: \t\t" + dto.username + "\nmy number:\t" + dto.tlfnumber.ToString() + "\nmy email:\t" + dto.email + "\nPassword:\t" + dto.password);
        var user = _service.Register(dto.username, dto.tlfnumber, dto.email, dto.password);
        return new ResponseDto
        {
            MessageToClient = "Successfully registered",
            ResponseData = user
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