using System.Security.Authentication;
using infrastructure.datamodels;
using infrastructure.mySqlRepositories;
using infrastructure.repositories;
using Microsoft.Extensions.Logging;

namespace service.accountservice;

public class AccountService
{
    private readonly PasswordHashRepository _passwordHashRepository;
    private readonly UserRepository _userRepository;
    private readonly UserRepo _userRepo;
    private readonly PasswordHashRepo _passwordHashRepo;

    public AccountService(
        UserRepo userRepo,
        UserRepository userRepository,
        PasswordHashRepository passwordHashRepository,
        PasswordHashRepo passwordHashRepo)
    {
        _userRepo = userRepo;
        _passwordHashRepo = passwordHashRepo;
        _userRepository = userRepository;
        _passwordHashRepository = passwordHashRepository;
    }

    /**
     * Called on login, and is used for authentication of the user, that is trying to login.
     * It is taking in email, and password in a clean string, and hashes the password,
     * to see if it is the same one in the DB.
     * Returns the user, so the User-info can be used in fx frontend
     */
    public User? Authenticate(string email, string password)
    {
        try
        {
            var passwordHash =
                _passwordHashRepo.GetByEmail(email); //Call Infrastructure to get the PasswordHash from the Email
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm); //Creates the hashing algorithm
            //Using the algorithm we just created, we try to validate it.
            //It takes in the password and salt, hashes it, and it takes the hashed from the db, and returns, if they match or not.
            var isValid = hashAlgorithm.VerifyHashedPassword(password, passwordHash.Hash, passwordHash.Salt);
            if (isValid) return _userRepo.GetById(passwordHash.UserId);
        }
        catch (Exception e)
        {
            throw new AuthenticationException("could not validate", e);
        }

        throw new InvalidCredentialException("Invalid credential!");
    }

    /**
     * A request is sent, and the information is stored
     * It creates the hashAlgorithm, salt and thereby the hashed password
     */
    public User Register(string username, int tlfnumber, string email, string password)
    {
        
        var hashAlgorithm = PasswordHashAlgorithm.Create();
        var salt = hashAlgorithm.GenerateSalt();
        var hash = hashAlgorithm.HashPassword(password, salt);
        //var user = _userRepository.Create(username, tlfnumber, email);
        //_passwordHashRepository.Create(user.id, hash, salt, hashAlgorithm.GetName());
        
        var user = _userRepo.Create(username, tlfnumber, email);
        _passwordHashRepo.Create(user.id, hash, salt, hashAlgorithm.GetName());
        return user;
    }

    public object Get(SessionData data)
    {
        return _userRepo.GetById(data.UserId);
    }
}