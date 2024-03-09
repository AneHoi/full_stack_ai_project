using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace service.accountservice;

public class JwtService
{
    private readonly JwtOptions _options;

    //constructor that takes in a Token as parameter 
    public JwtService(JwtOptions options)
    {
        _options = options;
    }

    //The algorithm the applications is using to sign the tokens
    private const string SignatureAlgorithm = SecurityAlgorithms.HmacSha512;

    public string IssueToken(SessionData data)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var token = jwtHandler.CreateEncodedJwt(new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_options.Secret),
                SignatureAlgorithm
            ),
            //Tokens can be issued by one system and used by another (audience)
            Issuer = _options.Address,
            Audience = _options.Address,
            //Expires is when the token is valid until.
            Expires = DateTime.UtcNow.Add(_options.Lifetime),
            //Claims are the payload of the token.
            Claims = data.ToDictionary()
        });
        return token;
    }

    /**
     * It should only accepts tokens signed by the one algorithm our applications is using.
     */
    public SessionData ValidateAndDecodeToken(string token)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        var principal = jwtHandler.ValidateToken(token, new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(_options.Secret),
            ValidAlgorithms = new[] { SignatureAlgorithm },

            // Default value is true already.
            // They are just set here to emphasise the importance.
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,

            ValidAudience = _options.Address,
            ValidIssuer = _options.Address,

            // Set to 0 when validating on the same system that created the token
            ClockSkew = TimeSpan.FromSeconds(0)
        }, out var securityToken);
        return SessionData.FromDictionary(new JwtPayload(principal.Claims));
    }
}