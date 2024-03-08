using service.accountservice;

namespace api;

/**
 * Getters and setters for the session data
 */
public static class HttpContextExtentions
{
    public static void SetSessionData(this HttpContext httpContext, SessionData data)
    {
        //Exists ONLY for the current request, and destroyed afterwards 
        httpContext.Items["data"] = data;
    }

    public static SessionData? GetSessionData(this HttpContext httpContext)
    {
        //Exists ONLY for the current request, and destroyed afterwards 
        return httpContext.Items["data"] as SessionData;
    }
}