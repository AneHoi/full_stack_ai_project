using infrastructure.datamodels;

namespace service.accountservice;

public class SessionData
{
    public required int UserId { get; init; }
    
    /**
    * When a user object is provided, a new instance of the SessionData is created.
    * It can be used from other classes, like this:
       User user = new User {Id = 456};
       SessionData session = SessionData.FromUser(user);
    * Creating a SessionData object from the User object
    */
    public static SessionData FromUser(User user)
    {
        return new SessionData { UserId = user.id };
    }
    
    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object> { { Keys.UserId, UserId }};
    }
    public static SessionData FromDictionary(Dictionary<string, object> dict)
    {
        return new SessionData { UserId = (int)dict[Keys.UserId] };
    }
    public static class Keys
    {
        public const string UserId = "u";
    }
}