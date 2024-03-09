using System.ComponentModel.DataAnnotations;

namespace api.dtoModels;

public class RegisterDto
{
    [Required] public string username { get; set; }
    
    [Required] public int tlfnumber { get; set; }

    [Required] public string email { get; set; }

    
    [Required]
    [MinLength(8)]
    public string password { get; set; }
}