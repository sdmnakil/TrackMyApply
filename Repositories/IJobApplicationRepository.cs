using JobApplicationTracker.Models;

namespace JobApplicationTracker.Repositories;

public interface IJobApplicationRepository
{
    Task<(List<JobApplication> Items, int TotalCount)> GetAllAsync(
        int userId,
        int page,
        int pageSize,
        string? sortBy,
        string? sortOrder);

    Task<JobApplication?> GetByIdAsync(
        int id,
        int userId);

    Task<List<JobApplication>> SearchAsync(
        int userId,
        string? company,
        string? status,
        string? sortBy,
        string? sortOrder);

    Task AddAsync(JobApplication jobApplication);

    Task UpdateAsync(JobApplication jobApplication);

    Task DeleteAsync(JobApplication jobApplication);
}