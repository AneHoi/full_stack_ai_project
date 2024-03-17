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
        var userId = HttpContext.GetSessionData().UserId;

        bool success = _profileService.SaveAllergens(allergens, userId);
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