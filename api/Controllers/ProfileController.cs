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
        
        //TODO send et response til frontend?
    }

    [HttpGet]
    [Route("api/getAllergens")]
    public IEnumerable<Allergen> GetAllergens()
    {
        return _profileService.GetAllergenCategories();
    }

    [HttpGet]
    [Route("api/getUsersAllergens")]
    public IEnumerable<int> GetUsersAllergens()
    {
        var userId = HttpContext.GetSessionData().UserId;

        return _profileService.GetUsersAllergens(userId);
    }
}