namespace CareerGuidance.Api.DTO
{
    public record  progressBarRequest
        (
             Guid id,
             string UserId,
             string roadmapId,
             string roadmapName,
             int progressValue,
             List<string> completedNodes

        );

   
            
         

}
