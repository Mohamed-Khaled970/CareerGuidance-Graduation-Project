namespace CareerGuidance.Api.DTO
{
    /*
     * File Name: RegisterUserDto.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - None
     * Contributors: Mohamed Khaled , Abdelrahman Rezq
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This class represents the Data Transfer Object (DTO) used for user registration.
     *      It includes all the necessary information required for registering a new user.
     */
    public class RegisterUserDto
    {
        /*
         * Property: Name
         * Description: Represents the full name of the user.
         * Type: string
         */
        // public string Name { get; set; } = string.Empty;

        /*
         * Property: UserName
         * Description: Represents the username for the user account.
         * Type: string
         */
        public string UserName { get; set; } = string.Empty;

        /*
         * Property: Password
         * Description: Represents the password for the user account.
         * Type: string
         */
        public string Password { get; set; } = string.Empty;

        /*
         * Property: ConfirmPassword
         * Description: Represents the confirmation of the user's password.
         * Type: string
         */
        public string ConfirmPassword { get; set; } = string.Empty;

        /*
         * Property: Email
         * Description: Represents the email address of the user.
         * Type: string
         */
        public string Email { get; set; } = string.Empty;

        /*
         * Property: PhoneNumber
         * Description: Represents the phone number of the user.
         * Type: string
         */
        //  public string PhoneNumber { get; set; } = string.Empty;

        /*
         * Property: Country
         * Description: Represents the country where the user resides.
         * Type: string
         */
        //  public string Country { get; set; } = string.Empty;

        /*
         * Property: City
         * Description: Represents the city where the user resides.
         * Type: string
         */
        //  public string City { get; set; } = string.Empty;

        /*
         * Property: Role
         * Description: Represents the role assigned to the user (e.g., student, instructor).
         * Type: string
         */
        //  public string Role { get; set; } = string.Empty;

        /*
         * Property: Age
         * Description: Represents the age of the user.
         * Type: int
         */
        // public int Age { get; set; }
    }
}