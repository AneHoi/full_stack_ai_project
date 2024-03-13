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
    public async Task<IActionResult> ReadFromImage([FromForm] IFormFile image)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

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

            return Ok("Image saved successfully!");
        }
        catch (Exception ex)
        {
            // Handle any errors
            return StatusCode(500, "Error saving image: " + ex.Message);
        }
    }
}