using api.dtoModels;
using ConsoleApp1.JsonFileExtractor;
using Microsoft.AspNetCore.Mvc;
using service.accountservice;
using service.allergenService;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]

public class AdminDbController : ControllerBase
{
    
    
    private readonly AllergenDbCreatorService _service;

    public AdminDbController(AllergenDbCreatorService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("/allergens/createdb")]
    public bool CreateAllergenDb()
    {

        _service.SaveCategories();
        
        _service.SaveAllergens();
        return true;//todo should be true if succes created 
    }
    
}