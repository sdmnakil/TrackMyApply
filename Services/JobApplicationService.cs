using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repositories;

namespace JobApplicationTracker.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _repository;

    public JobApplicationService(
        IJobApplicationRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResultDto<JobApplicationDto>> GetAllAsync(
        int userId,
        int page,
        int pageSize,
        string? sortBy,
        string? sortOrder)
    {
        var (items, totalCount) = await _repository.GetAllAsync(
            userId,
            page,
            pageSize,
            sortBy,
            sortOrder);

        return new PagedResultDto<JobApplicationDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<JobApplicationDto?> GetByIdAsync(
        int id,
        int userId)
    {
        var application =
            await _repository.GetByIdAsync(id, userId);

        if (application == null)
        {
            return null;
        }

        return MapToDto(application);
    }

    public async Task<JobApplicationDto> AddAsync(
        int userId,
        CreateJobApplicationDto dto)
    {
        var application = new JobApplication
        {
            UserId = userId,

            CompanyName = dto.CompanyName,
            JobTitle = dto.JobTitle,
            JobType = dto.JobType,
            Location = dto.Location,

            SalaryMin = dto.SalaryMin,
            SalaryMax = dto.SalaryMax,

            IsSalaryNegotiable = dto.IsSalaryNegotiable,

            ApplicationDate = dto.ApplicationDate,

            Status = dto.Status,
            JobUrl = dto.JobUrl,
            Notes = dto.Notes
        };

        await _repository.AddAsync(application);

        return MapToDto(application);
    }

    public async Task<bool> UpdateAsync(
        int id,
        int userId,
        UpdateJobApplicationDto dto)
    {
        var application =
            await _repository.GetByIdAsync(id, userId);

        if (application == null)
        {
            return false;
        }

        application.CompanyName = dto.CompanyName;
        application.JobTitle = dto.JobTitle;
        application.JobType = dto.JobType;
        application.Location = dto.Location;

        application.SalaryMin = dto.SalaryMin;
        application.SalaryMax = dto.SalaryMax;

        application.IsSalaryNegotiable =
            dto.IsSalaryNegotiable;

        application.ApplicationDate =
            dto.ApplicationDate;

        application.Status = dto.Status;
        application.JobUrl = dto.JobUrl;
        application.Notes = dto.Notes;

        await _repository.UpdateAsync(application);

        return true;
    }

    public async Task<bool> DeleteAsync(
        int id,
        int userId)
    {
        var application =
            await _repository.GetByIdAsync(id, userId);

        if (application == null)
        {
            return false;
        }

        await _repository.DeleteAsync(application);

        return true;
    }

    public async Task<List<JobApplicationDto>> SearchAsync(
        int userId,
        string? company,
        string? status,
        string? sortBy,
        string? sortOrder)
    {
        var applications =
            await _repository.SearchAsync(
                userId,
                company,
                status,
                sortBy,
                sortOrder);

        return applications
            .Select(MapToDto)
            .ToList();
    }

    private static JobApplicationDto MapToDto(
        JobApplication application)
    {
        return new JobApplicationDto
        {
            Id = application.Id,

            CompanyName = application.CompanyName,
            JobTitle = application.JobTitle,
            JobType = application.JobType,
            Location = application.Location,

            SalaryMin = application.SalaryMin,
            SalaryMax = application.SalaryMax,

            IsSalaryNegotiable =
                application.IsSalaryNegotiable,

            ApplicationDate =
                application.ApplicationDate,

            Status = application.Status,
            JobUrl = application.JobUrl,
            Notes = application.Notes
        };
    }
}