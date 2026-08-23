using JobApplicationTracker.Data;
using JobApplicationTracker.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetStatisticsAsync(int userId)
    {
        var applications = await _context.JobApplications
            .Where(a => a.UserId == userId)
            .ToListAsync();

        var dashboard = new DashboardDto
        {
            TotalApplications = applications.Count,
            Applied = applications.Count(a => a.Status == "Applied"),
            Interview = applications.Count(a => a.Status == "Interview"),
            Assessment = applications.Count(a => a.Status == "Assessment"),
            Rejected = applications.Count(a => a.Status == "Rejected"),
            Offer = applications.Count(a => a.Status == "Offer")
        };

        return dashboard;
    }
}