namespace JobApplicationTracker.Exceptions;

// Kono resource (jemon: ekta specific job application) na paile
// ei exception throw kora hobe. Middleware eta dhore 404 return korbe.
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}