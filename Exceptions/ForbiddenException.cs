namespace JobApplicationTracker.Exceptions;

// Ekjon user onno kono user-er data access korar chesta korle
// ei exception throw kora hobe. Middleware eta dhore 403 return korbe.
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message)
    {
    }
}