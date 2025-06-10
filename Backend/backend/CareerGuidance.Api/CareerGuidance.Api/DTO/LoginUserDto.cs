namespace CareerGuidance.Api.DTO
{
    /*
     * File Name: LoginUserDto.cs
     * Author Information: Mohamed Khaled
     * Date of creation: 2024-08-09
     * Versions Information: v1.0
     * Dependencies:
     *      - None
     * Contributors: Mohamed Khaled
     * Last Modified Date: 2024-08-19
     *
     * Description:
     *      This class represents a record used for user login information.
     *      It is used to transfer the email and password for user authentication.
     *      This record provides a concise way to encapsulate login credentials.
     */
    public record LoginUserDto
    (
        /*
         * Property: Email
         * Description: Represents the email address of the user for login purposes.
         * Type: string
         */
        string EmailOrUsername,

        /*
         * Property: Password
         * Description: Represents the password of the user for login purposes.
         * Type: string
         */
        string Password
    );
}
