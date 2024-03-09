using api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace test;

[TestFixture]
public class AccountControllerTest
{
    private AccountController _accountController;
    
    [SetUp]
    public void Setup()
    {
        // Create an instance of your controller
        _accountController = new AccountController();
    }

    [Test]
    public void TestRegister()
    {
        //Arrange
        string name = "Henriette";
        
        //Act
        var result = _accountController.Register(name);
        
        //Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("welcome", result);
    }
}