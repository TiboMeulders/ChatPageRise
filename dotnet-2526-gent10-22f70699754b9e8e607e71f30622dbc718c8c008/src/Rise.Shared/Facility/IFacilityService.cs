using Rise.Shared.Common;

namespace Rise.Shared.Facility;

public interface IFacilityService
{
    Task<Result<FacilityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default);
}