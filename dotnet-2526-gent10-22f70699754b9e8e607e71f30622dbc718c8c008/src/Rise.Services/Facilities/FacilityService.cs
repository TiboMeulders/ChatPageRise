using Microsoft.EntityFrameworkCore;
using Rise.Domain.Facility;
using Rise.Persistence;
using Rise.Shared.Common;
using Rise.Shared.Facility;
using Rise.Shared.Identity.Accounts;

namespace Rise.Services.Facilities;

public class FacilityService(ApplicationDbContext dbContext) : IFacilityService
{
    public async Task<Result<FacilityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var query = dbContext.Facilities.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(a => a.Name.Contains(request.SearchTerm));
        }

        var totalCount = await query.CountAsync(ctx);

        // Apply ordering
        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            query = request.OrderDescending
                ? query.OrderByDescending(e => EF.Property<object>(e, request.OrderBy))
                : query.OrderBy(e => EF.Property<object>(e, request.OrderBy));
        }
        else
        {
            // Default order
            query = query.OrderBy(a => a.Name);
        }
        var facilities = await query.AsNoTracking()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(a => new FacilityDto.Index()
            {
                Id = a.Id,
                Name = a.Name
            })
            .ToListAsync(ctx);

        return Result.Success(new FacilityResponse.Index
        {
            Facilities = facilities,
            TotalCount = totalCount
        });
    }
}