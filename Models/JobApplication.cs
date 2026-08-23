namespace JobApplicationTracker.Models;

public class JobApplication
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string JobType { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal? SalaryMin { get; set; }

    public decimal? SalaryMax { get; set; }

    public bool IsSalaryNegotiable { get; set; }

    public DateTime ApplicationDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string JobUrl { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}