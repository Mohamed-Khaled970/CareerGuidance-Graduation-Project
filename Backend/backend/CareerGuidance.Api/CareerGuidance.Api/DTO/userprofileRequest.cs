namespace CareerGuidance.Api.DTO
{
    public record userprofileRequest
        (
            string Name,
            string PhoneNumber,
            string ImageUrl,
            string Country,
            DateTime DateOfBirth,


             // Social Media Links
             string? Instagram,
             string? Facebook,
             string? GitHub,
             string? LinkedIn

        );
    

}
