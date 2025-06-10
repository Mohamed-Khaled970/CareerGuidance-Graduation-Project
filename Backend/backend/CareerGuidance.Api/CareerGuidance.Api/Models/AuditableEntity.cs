using System.Text.Json.Serialization;

namespace CareerGuidance.Api.Models
{
    /*
     * File Name: AuditableEntity.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - using System; // For DateTime
     *      - using System.Text.Json.Serialization; // For JsonIgnore attribute
     *      - using CareerGuidance.Api.Models; // Assuming ApplicationUser is defined here
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     */

    public class AuditableEntity
    {
        /*
         * Property: CreatedById
         * Description: Identifier for the user who created the entity.
         */
        public string CreatedById { get; set; } = string.Empty;

        /*
         * Property: CreatedOn
         * Description: Date and time when the entity was created.
         */
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /*
         * Property: UpdatedById
         * Description: Identifier for the user who last updated the entity.
         * Nullable: Yes
         */
        public string? UpdatedById { get; set; }

        /*
         * Property: UpdatedOn
         * Description: Date and time when the entity was last updated.
         * Nullable: Yes
         */
        public DateTime? UpdatedOn { get; set; }

        /*
         * Property: CreatedBy
         * Description: Navigation property for the user who created the entity.
         * JsonIgnore: This property will be ignored during serialization.
         */
        [JsonIgnore]
        public ApplicationUser CreatedBy { get; set; } = default!;

        /*
         * Property: UpdatedBy
         * Description: Navigation property for the user who last updated the entity.
         * JsonIgnore: This property will be ignored during serialization.
         */
        [JsonIgnore]
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
