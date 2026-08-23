using JobApplicationTracker.DTOs;

namespace JobApplicationTracker.Services;

public interface IJobApplicationService
{
    Task<PagedResultDto<JobApplicationDto>> GetAllAsync(
        int userId,
        int page,
        int pageSize,
        string? sortBy,
        string? sortOrder);

    Task<JobApplicationDto?> GetByIdAsync(
        int id,
        int userId);

    Task<JobApplicationDto> AddAsync(
        int userId,
        CreateJobApplicationDto dto);

    Task<bool> UpdateAsync(
        int id,
        int userId,
        UpdateJobApplicationDto dto);

    Task<bool> DeleteAsync(
        int id,
        int userId);

    Task<List<JobApplicationDto>> SearchAsync(
        int userId,
        string? company,
        string? status,
        string? sortBy,
        string? sortOrder);
}