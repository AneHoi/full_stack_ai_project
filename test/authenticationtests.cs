using api.Controllers;
using api.dtoModels;
using infrastructure.repositories;
using Microsoft.Extensions.Logging;
using Moq;
using service.accountservice;

namespace test;

[TestFixture]
public class Tests
{
    private AccountController _accountController;
    private Mock<AccountService> _service;
    private Mock<JwtService> _jwtService;

    private Mock<ILogger<AccountService>> _logger;
    private Mock<PasswordHashRepository> _passwordHashRepository;
    private Mock<UserRepository> _userRepository;
    [SetUp]
    public void Setup()
    {
        /*_logger = new Mock<ILogger<AccountService>>(null);
        _passwordHashRepository = new Mock<PasswordHashRepository>(null);
        _userRepository = new Mock<UserRepository>(null);
        *///_service = new Mock<AccountService>(_logger.Object, _userRepository.Object, _passwordHashRepository.Object);
        _jwtService = new Mock<JwtService>();
        _service = new Mock<AccountService>();
        _accountController = new AccountController(_service.Object, _jwtService.Object);
    }

    [Test]
    public void Login()
    {
        LoginDto loginDto = new LoginDto
        {
            email = "test@mail",
            password = "strongPassword1234"
        };
        Console.WriteLine(1);
        var response = _accountController.Login(loginDto);
        Console.WriteLine(2);
        Assert.IsNotNull(response);
        Console.WriteLine(3);
        Assert.AreEqual("Successfully authenticated", response.MessageToClient);
        Console.WriteLine(4);

        // Depending on the structure of your ResponseDto, you may need to adjust this assertion
        Assert.IsNotNull(response.ResponseData);
        Assert.IsNotNull(response.ResponseData.ToString());
    }
}