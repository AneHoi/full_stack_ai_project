using ConsoleApp1.JsonFileExtractor;
using infrastructure.mySqlRepositories;

namespace service.allergenService;

public class AllergenDbCreatorService
{
    private string _allergensCategoryJsonPath = @"C:\Game\full_stack_ai_project\infrastructure\jsonRepositories\JsonFileExtractor\Categories.json";
    private string _allergensJsonPath = @"C:\Game\full_stack_ai_project\infrastructure\jsonRepositories\JsonFileExtractor\allegenWithCategory.json";
    
    private List<AllergenData> _allergens;
    private List<string> _categories;
    
    private AllergenRepo _allegenRepo;
    private ProductRepo _productRepo;
    private FoodJsonExtractorRepository _foodRepo;
    
    public AllergenDbCreatorService(AllergenRepo allergenRepo, ProductRepo productRepo, FoodJsonExtractorRepository foodRepo)
    {
        _allegenRepo = allergenRepo;
        _productRepo = productRepo;
        _foodRepo = foodRepo;
        
        _categories = AllergenJsonReader.ReadAllergenCategoryList(_allergensCategoryJsonPath);
        _allergens = AllergenJsonReader.ReadAllergens(_allergensJsonPath);
    }

    public void SaveCategories()
    {
        _allegenRepo.CreateCategoriesFromJsonList(_categories);
    }
    
    public void SaveAllergens()
    {
        foreach (var allergen in _allergens)
        {
            _allegenRepo.CreateAllergens(allergen);
        }
    }

    public void SaveProducts()
    {
        _foodRepo.ExtractNamesAndItems(@"C:\Users\kenni\Downloads\openfoodfacts-products.jsonl\openfoodfacts-products.jsonl");
    }
    
    
}