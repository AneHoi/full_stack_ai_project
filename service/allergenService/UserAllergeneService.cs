using infrastructure.datamodels;
using infrastructure.mySqlRepositories;

namespace service.allergenService;

public class UserAllergeneService
{
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

    public void isUserAllergicTo()
    {
        
    }
}