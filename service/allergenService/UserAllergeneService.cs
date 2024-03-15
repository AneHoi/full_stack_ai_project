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

    public List<string> isUserAllergicTo(string result, int userId)
    {
        List<string> strings = seperateBySpace(result);

        var b = _repo.GetUsersAllergens(userId);


        var f = _repo.CheckForAllergy(strings, b.ToList());
       return f;
    }
}