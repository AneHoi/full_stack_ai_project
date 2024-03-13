using System.Text;
using api.dtoModels;
using Newtonsoft.Json;

namespace test;

public class Tests
{
    private HttpClient _http;

    //running before EVERY test
    [SetUp]
    public void Setup()
    {
        _http = new HttpClient();
        
        Helper.TriggerRebuild();
    }


    [TestCase("tester", 12345678, "test@mail", "StrongPassword")]
    [TestCase("kenneth", 1237542, "kenneth@mail", "KStrongPassword")]
    [TestCase("Jonas", 12397654, "jonas@mail", "JStrongPassword")]
    public async Task RegisterTest(string username, int tlf, string mail, string password)
    {
        RegisterDto registerDto = new RegisterDto()
        {
            username = username,
            tlfnumber = tlf,
            email = mail,
            password = password
        };
        // Arrange
        var json = JsonConvert.SerializeObject(registerDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        string baseUri = "http://localhost:5096";


        // Act
        var response = await _http.PostAsync(baseUri + "/account/register", content);
        
        // Check if the response is successful
        response.EnsureSuccessStatusCode();
        // Deserialize the response content into the expected type
        ResponseDto responseObject = await response.Content.ReadAsAsync<ResponseDto>();

        //Assert
        // Check if the deserialized object is of the expected type
        Assert.IsInstanceOf<ResponseDto>(responseObject);
        Assert.IsNotNull(response);
        Assert.AreEqual("Successfully registered", responseObject.MessageToClient);
    }


    [Test]
    public async Task LoginTest()
    {
        //insert another user in SQL
        
        LoginDto loginDto = new LoginDto
        {
            email = "test@mail",
            password = "strongPassword1234"
        };
        // Arrange
        var json = JsonConvert.SerializeObject(loginDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        string baseUri = "http://localhost:5096";


        // Act
        var response = await _http.PostAsync(baseUri + "/account/login", content);

        Assert.IsNotNull(response);
        
    }

    [TearDownAttribute]
    public void TearDown()
    {
        _http.Dispose();
    }
}