namespace Rise.Shared.Facility;
public static partial class FacilityResponse
{
    public class Index
    {
        public IEnumerable<FacilityDto.Index> Facilities { get; set; } = [];
        public int TotalCount { get; set; }
    }
}   