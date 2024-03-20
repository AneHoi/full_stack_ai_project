using api.dtoModels;
using infrastructure.datamodels;
using Microsoft.AspNetCore.Mvc;
using service;
using service.allergenService;

namespace api.Controllers;

[ApiController]
public class ImageController : ControllerBase
{
    private readonly ComputerVisionService _computerVisionService;
    private readonly UserAllergeneService _userAllergeneService;

    public ImageController(ComputerVisionService computerVisionService,
        UserAllergeneService userAllergeneService)
    {
        _computerVisionService = computerVisionService;
        _userAllergeneService = userAllergeneService;
    }
    [HttpPost]
    [Route("api/analyze")]
    
    public async Task<ImageResultDto> ReadFromImage([FromForm] IFormFile image)
    {
        try
        {
            // Get the path to the system's temporary directory
            var tempPath = Path.GetTempPath();

            // Save the uploaded file to the temporary directory
            var imagePath = Path.Combine(tempPath, image.FileName);

            await using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            using (var computerResult = _computerVisionService.MakeRequest(imagePath))
            {
                var session = HttpContext.GetSessionData()!;
            
                var allergenResult = _userAllergeneService.isUserAllergicTo(computerResult.Result, session.UserId);
            
                return new ImageResultDto
                {
                    text = computerResult.Result,
                    allergenes = allergenResult
                };
            }
        }
        catch (Exception ex)
        {
            // Handle any errors
            throw new Exception("", ex);
        }
    }
}