using System.Text;
using Konscious.Security.Cryptography;

namespace service.cryptoAlgorithms;

public class Argon2idPasswordHashAlgorithm : PasswordHashAlgorithm
{
    public const string Name = "argon2id";

    public override string GetName() => Name;

    /**
     * Hashing the password here, by taking in the password and the randomly generated salt.
     * If the salt and password is allways the same, then the hashed password is also the same.
     */
    public override string HashPassword(string password, string salt)
    {
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = Decode(salt),
            MemorySize = 12288,
            Iterations = 3,
            DegreeOfParallelism = 1,
        };
        return Encode(hashAlgo.GetBytes(256));
    }

    /**
     * Checks if the given password matches the other hash.
     * SequenceEqual allows us to compare two sequences for equality. Example:
      int[] array1 = { 1, 2, 3 };
      int[] array2 = { 1, 2, 3 };
      bool result = array1.SequenceEqual(array2); // result: true
     */
    public override bool VerifyHashedPassword(string password, string hash, string salt)
    {
        return HashPassword(password, salt).SequenceEqual(hash);
    }
    
}