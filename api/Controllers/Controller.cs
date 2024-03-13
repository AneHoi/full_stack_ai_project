using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    [HttpPost]
    [Route("api/saveAllergens")]
    public void SaveAllergens(int[] allergens)
    {
        Console.WriteLine("Allergen[0]: "+allergens[0]);
        Console.WriteLine("Length: "+allergens.Length);

        //TODO
        //1) Find user id fra Jwt
        //2) Saml og send til service > repo
        //3) returner Ok?
    }
}
