namespace api.dtoModels;

/**
 * Primarily used for the users and the passwords
 */
public class ResponseDto
{
    public string MessageToClient { get; set; }
    public object? ResponseData { get; set; }
}