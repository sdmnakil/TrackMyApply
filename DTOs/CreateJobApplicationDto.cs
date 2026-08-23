using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JobApplicationTracker.DTOs;

public class CreateJobApplicationDto : IValidatableObject
{
    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(150, ErrorMessage = "Company name cannot exceed 150 characters.")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job title is required.")]
    [StringLength(150, ErrorMessage = "Job title cannot exceed 150 characters.")]
    public string JobTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Job type is required.")]
    public string JobType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Location is required.")]
    public string Location { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Minimum salary must be a positive number.")]
    public decimal? SalaryMin { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Maximum salary must be a positive number.")]
    public decimal? SalaryMax { get; set; }

    public bool IsSalaryNegotiable { get; set; }

    [Required(ErrorMessage = "Application date is required.")]
    public DateTime ApplicationDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Job URL cannot exceed 500 characters.")]
    public string JobUrl { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string Notes { get; set; } = string.Empty;

    // Ei method IValidatableObject theke ashe — eta diye
    // amra ekadhik property mile-je check korte pari (cross-field validation),
    // ja [Range] ba [Required] diye kora jay na.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SalaryMin.HasValue && SalaryMax.HasValue && SalaryMax < SalaryMin)
        {
            yield return new ValidationResult(
                "Maximum salary cannot be less than minimum salary.",
                new[] { nameof(SalaryMax) });
        }

        if (ApplicationDate > DateTime.UtcNow.AddDays(1))
        {
            yield return new ValidationResult(
                "Application date cannot be in the future.",
                new[] { nameof(ApplicationDate) });
        }

        if (!JobApplicationStatus.AllowedValues.Contains(Status))
        {
            yield return new ValidationResult(
                $"Status must be one of: {string.Join(", ", JobApplicationStatus.AllowedValues)}.",
                new[] { nameof(Status) });
        }
    }
}