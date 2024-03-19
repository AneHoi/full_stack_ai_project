using infrastructure.datamodels;
using infrastructure.mySqlRepositories;

namespace service.allergenService;

public class UserAllergeneService
{
    private AllergenRepo _repo;
    public UserAllergeneService(AllergenRepo repo)
    {
        _repo = repo;
    }
    
    private AllergenRepo _repository;

    private List<string> seperateBySpace(string ingredients)
    {
        List<string> ingredientlist = new List<string>();

        foreach (var ingredient in ingredients.Split(' '))
        {
            ingredientlist.Add(ingredient);
        }
        return ingredientlist;
    }

    public List<AllergenWithCategoryDto> isUserAllergicTo(string result, int userId)
    {
        List<string> strings = seperateBySpace(result);

        var userAllergyList = _repo.GetUsersAllergens(userId);

        
        var  allergenIngredients= _repo.CheckForAllergy(strings, userAllergyList.ToList());
       return allergenIngredients;
    }
}