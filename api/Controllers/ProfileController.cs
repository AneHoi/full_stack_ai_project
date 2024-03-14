using api.filter;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service.profileService;

namespace api.Controllers;

[ApiController]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;
    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [RequireAuthentication]
    [HttpPost]
    [Route("api/saveAllergens")]
    public void SaveAllergens(int[] allergens)
    {
        Console.WriteLine("Allergen[0]: "+allergens[0]);
        Console.WriteLine("Length: "+allergens.Length);

        var userId = HttpContext.GetSessionData().UserId;
        Console.WriteLine("User ID: "+userId);

        bool success = _profileService.SaveAllergens(allergens, userId);
        Console.WriteLine("Success: "+success);
        
        //1) Find user id fra Jwt //TODO hmmm something seems off. Virker fra swagger, ikke fra frontend
        //2) Saml og send til service > repo //½done. TODO: Tag højde for unticked. Dvs. hvis man fravælger igen skal den fjernes. Lige nu gemmes alle valgte bare.
        //3) returner Ok? 
    }

    [HttpGet]
    [Route("api/getAllergens")]
    public IEnumerable<Allergen> GetAllergens()
    {
        return _profileService.GetAllergenCategories();
    }
}