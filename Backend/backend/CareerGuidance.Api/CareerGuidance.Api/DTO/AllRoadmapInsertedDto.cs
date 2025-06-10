namespace CareerGuidance.Api.DTO
{
    /*
   * File Name: allRoadmapInsertedDto.cs
   * Author Information: Abdelrahman Rezk
   * Date of creation: 2024-10-13
   * Versions Information: v1.0
   * Dependencies: None
   * Contributors: Abdelrahman Rezk
   * Last Modified Date: 2024-10-27
   *
   * Description:
   *      take roadmap data from user 
   */
    public class allRoadmapInsertedDto
    {
        [Required]
        public string roadmapData { get; set; } = string.Empty;
    }
}
