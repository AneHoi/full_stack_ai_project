using infrastructure.datamodels;
using infrastructure.mySqlRepositories;

namespace service.profileService;

public class ProfileService
{
    private readonly AllergenRepo _allergenRepo;

    public ProfileService(AllergenRepo allergenRepo)
    {
        _allergenRepo = allergenRepo;
    }

    public IEnumerable<Allergen> GetAllergenCategories()
    {
        return _allergenRepo.GetAllergenCategories();
    }

    public bool SaveAllergens(IEnumerable<int> allergens, int user_id)
    {
        return _allergenRepo.SaveAllergens(allergens, user_id);
    }

    public IEnumerable<int> GetUsersAllergens(int userId)
    {
        return _allergenRepo.GetUsersAllergens(userId);
    }
}