using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Rise.Shared.Common;
using Rise.Shared.Facility;

namespace Rise.Client.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly HttpClient _httpClient;

        public FacilityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<FacilityResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
        {
            var queryParams = $"?Skip={request.Skip}&Take={request.Take}";
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                queryParams += $"&SearchTerm={Uri.EscapeDataString(request.SearchTerm)}";
            if (!string.IsNullOrWhiteSpace(request.OrderBy))
                queryParams += $"&OrderBy={Uri.EscapeDataString(request.OrderBy)}&OrderDescending={request.OrderDescending}";

            var response = await _httpClient.GetFromJsonAsync<Result<FacilityResponse.Index>>(
                $"api/facilities{queryParams}", ctx);

            return response!;
        }

    }
}