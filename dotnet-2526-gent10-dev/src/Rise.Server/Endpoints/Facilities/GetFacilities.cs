using Rise.Shared.Common;
using Rise.Shared.Facility;

namespace Rise.Server.Endpoints.Facilities;

public class GetFacilities(IFacilityService facilityService) : Endpoint<QueryRequest.SkipTake, Result<FacilityResponse.Index>>
{
    public override void Configure()
    {
        Get("/api/facilities");
        AllowAnonymous();
    }

    public override Task<Result<FacilityResponse.Index>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        return facilityService.GetIndexAsync(req, ct);
    }
}