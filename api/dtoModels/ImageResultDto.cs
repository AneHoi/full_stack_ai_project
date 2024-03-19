using infrastructure.mySqlRepositories;

namespace api.dtoModels;

public class ImageResultDto
{
    public string text { get; set; }
    public List<AllergenWithCategoryDto> allergenes { get; set; }
}