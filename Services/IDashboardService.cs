using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IDashboardService
{
    Task<DashboardDto> GetStatisticsAsync(int userId);
}