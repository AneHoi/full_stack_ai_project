using System.Security.Cryptography;
using service.cryptoAlgorithms;

namespace service;


public abstract class PasswordHashAlgorithm
{
    //We set the preferred hashing algorithm to the name that is given in this class: Argon2idPasswordHashAlgorithm
    const string PreferredAlgorithmName = Argon2idPasswordHashAlgorithm.Name;

    //Here we create instances of the prefered hashing algorithm
    public static PasswordHashAlgorithm Create(string algorithmName = PreferredAlgorithmName)
    {
        switch (algorithmName)
        {
            case Argon2idPasswordHashAlgorithm.Name:
                return new Argon2idPasswordHashAlgorithm();
            default:
                throw new NotImplementedException();
        }
    }
    public abstract string GetName();

    public abstract string HashPassword(string password, string salt);

    public abstract bool VerifyHashedPassword(string password, string hash, string salt);

    public string GenerateSalt()
    {
        return Encode(RandomNumberGenerator.GetBytes(128));
    }

    //Converts the salt to a usefull byte array, to use it in the encoding later
    protected byte[] Decode(string value)
    {
        return Convert.FromBase64String(value);
    }

    protected string Encode(byte[] value)
    {
        return Convert.ToBase64String(value);
    }
}