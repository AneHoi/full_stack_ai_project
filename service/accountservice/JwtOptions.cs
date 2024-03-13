namespace service.accountservice;

public class JwtOptions
{
    public required byte[] Secret { get; init; }
    //TimeSpan is a data-type, that measures how much time it has to live
    public required TimeSpan Lifetime { get; init; }
    public string? Address { get; set; }
}