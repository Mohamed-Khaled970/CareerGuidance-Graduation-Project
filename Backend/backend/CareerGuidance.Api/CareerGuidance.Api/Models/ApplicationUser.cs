using CareerGuidance.Api.Migrations;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CareerGuidance.Api.Models
{
    /*
     * File Name: ApplicationUser.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - using Microsoft.AspNetCore.Identity; // For IdentityUser base class
     * Contributors: Mohamed Khaled - abdelrahman maged
     * Last Modified Date: 2024-08-19
     */

    public class ApplicationUser : IdentityUser
    {
        /*
         * Property: Name
         * Description: Represents the name of the user.
         * Type: string
         */
        public string Name { get; set; } = string.Empty;

        /*
         * Property: Country
         * Description: Represents the country of the user.
         * Type: string
         */
        public string Country { get; set; } = string.Empty;


        /*
         * Property: Role
         * Description: Represents the role of the user (e.g., student, instructor).
         * Type: string
         */
        public string Role { get; set; } = string.Empty;

        
        /*
        * Property: Date of birth
        * Description: Represents the age of the user.
        * Type: date time 
        */

        public DateTime DateOfBirth { get; set; }


        /*
         * Property: image url
         * Description: Represents the image of the user.
         * Type: string 
         */
        public string ImageUrl { get; set; } = string.Empty;

        

        /* social media */
        public socialMedia SocialMedia { get; set; }


       // public ICollection<progressBar> ProgressBars { get; set; }
        public ICollection<progressBar> ProgressBars { get; set; } = new List<progressBar>();



        public List<RefreshToken> RefreshTokens { get; set; } = [];
        
     

    }
}
