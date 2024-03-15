using DefaultNamespace;
using api.dtoModels;
using Microsoft.AspNetCore.Mvc;
using service;

namespace api.Controllers;

[ApiController]
public class ImageController : ControllerBase
{
    private readonly ComputerVisionService _computerVisionService;

    public ImageController(ComputerVisionService computerVisionService)
    {
        _computerVisionService = computerVisionService;
    }
    [HttpPost]
    [Route("api/analyze")]
    public async Task<ImageResultDto> ReadFromImage([FromForm] IFormFile image)
    {
        try
        {
            // Get the path to the system's temporary directory
            var tempPath = Path.GetTempPath();
            Console.WriteLine("Path: "+tempPath);

            // Save the uploaded file to the temporary directory
            var imagePath = Path.Combine(tempPath, image.FileName);
            
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            
            _computerVisionService.MakeRequest(imagePath);

            return new ImageResultDto
            {
                text = "hej jeg elsker øl",
                allergenes = new List<string> { "hellominven", "sewibuddy" }
            };
        }
        catch (Exception ex)
        {
            // Handle any errors
            throw new Exception();
        }
    }
}