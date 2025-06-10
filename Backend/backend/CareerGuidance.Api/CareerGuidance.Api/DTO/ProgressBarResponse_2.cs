namespace CareerGuidance.Api.DTO
{
    public record ProgressBarResponse_2
        (
             Guid Id,
             string UserId,
             string roadmapId,
             string roadmapName,
             int progressValue,
             List<string> completedNodes
        );
    
    
}
