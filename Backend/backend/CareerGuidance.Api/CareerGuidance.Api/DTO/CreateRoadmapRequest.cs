namespace CareerGuidance.Api.DTO
{
    public record CreateRoadmapRequest
    (
        string Category,
        string RoadmapName,
        string Discription,
        string ImageUrl
    );
}
