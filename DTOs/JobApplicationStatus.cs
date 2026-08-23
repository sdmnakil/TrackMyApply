namespace JobApplicationTracker.DTOs;

// Ekta central jaygay shob allowed status value rakha holo,
// jate future-e notun status add korte hole shudhu ekhanei change lage.
public static class JobApplicationStatus
{
    public static readonly string[] AllowedValues =
    {
        "Applied",
        "Interview",
        "Assessment",
        "Rejected",
        "Offer"
    };
}