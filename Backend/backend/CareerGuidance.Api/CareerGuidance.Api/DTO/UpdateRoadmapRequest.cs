namespace CareerGuidance.Api.DTO
{
    public record UpdateRoadmapRequest
    (
        string Category,
        string RoadmapName,
        string Discription,
        string ImageUrl
    );
}
