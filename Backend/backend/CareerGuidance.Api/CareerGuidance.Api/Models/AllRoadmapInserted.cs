using System.ComponentModel.DataAnnotations.Schema;

namespace CareerGuidance.Api.Models
{
    /*
     * File Name: allRoadmapInserted.cs
     * Author Information: Abdelrahman Rezk
     * Date of creation: 2024-10-13
     * Versions Information: v1.0
     * Dependencies: None
     * Contributors: Abdelrahman Rezk
     * Last Modified Date: 2024-10-27
     *
     * Description:
     *      roadmap data inserted ini database by id 
     */
    public class allRoadmapInserted
    {
        public Guid Id { get; set; }
        [Required]
        public string roadmapData { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; } //FK


        [ForeignKey("CategoryId")]
        public roadmapCategory Categories { get; set; }  //Relation
      


    }
}
