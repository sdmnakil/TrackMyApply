using JobApplicationTracker.Data;
using JobApplicationTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public JobApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(List<JobApplication> Items, int TotalCount)> GetAllAsync(
        int userId,
        int page,
        int pageSize,
        string? sortBy,
        string? sortOrder)
    {
        var query = _context.JobApplications
            .Where(x => x.UserId == userId)
            .AsQueryable();

        // Page-e dhukanor age total count ber kore ne, jate
        // pagination metadata (totalPages) accurate hoy.
        var totalCount = await query.CountAsync();

        query = ApplySorting(query, sortBy, sortOrder);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<JobApplication?> GetByIdAsync(
        int id,
        int userId)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == userId);
    }

    public async Task<List<JobApplication>> SearchAsync(
        int userId,
        string? company,
        string? status,
        string? sortBy,
        string? sortOrder)
    {
        var query = _context.JobApplications
            .Where(x => x.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(company))
        {
            query = query.Where(x =>
                x.CompanyName.Contains(company));
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x =>
                x.Status == status);
        }

        query = ApplySorting(query, sortBy, sortOrder);

        return await query.ToListAsync();
    }

    // Sorting logic ekjaygay centralize kora holo, jate GetAllAsync
    // ar SearchAsync duitatei same code repeat na hoy.
    private static IQueryable<JobApplication> ApplySorting(
        IQueryable<JobApplication> query,
        string? sortBy,
        string? sortOrder)
    {
        bool descending = string.Equals(
            sortOrder,
            "desc",
            StringComparison.OrdinalIgnoreCase);

        switch (sortBy?.ToLower())
        {
            case "companyname":
                query = descending
                    ? query.OrderByDescending(x => x.CompanyName)
                    : query.OrderBy(x => x.CompanyName);
                break;

            case "jobtitle":
                query = descending
                    ? query.OrderByDescending(x => x.JobTitle)
                    : query.OrderBy(x => x.JobTitle);
                break;

            case "salary":
                query = descending
                    ? query.OrderByDescending(x => x.SalaryMin)
                    : query.OrderBy(x => x.SalaryMin);
                break;

            case "applicationdate":
            default:
                query = descending
                    ? query.OrderByDescending(x => x.ApplicationDate)
                    : query.OrderBy(x => x.ApplicationDate);
                break;
        }

        return query;
    }

    public async Task AddAsync(JobApplication jobApplication)
    {
        await _context.JobApplications.AddAsync(jobApplication);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobApplication jobApplication)
    {
        _context.JobApplications.Update(jobApplication);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(JobApplication jobApplication)
    {
        _context.JobApplications.Remove(jobApplication);
        await _context.SaveChangesAsync();
    }
}