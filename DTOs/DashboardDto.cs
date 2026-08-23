namespace JobApplicationTracker.DTOs;

public class DashboardDto
{
    public int TotalApplications { get; set; }
    public int Applied { get; set; }
    public int Interview { get; set; }
    public int Assessment { get; set; }
    public int Rejected { get; set; }
    public int Offer { get; set; }
}