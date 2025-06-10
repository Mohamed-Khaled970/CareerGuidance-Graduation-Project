namespace CareerGuidance.Api.DTO
{

    
    public record userprofileResponse
        (
            string UserName,
            string PasswordHash,
            string Email,
            string Name,
            string PhoneNumber,
            string Role,
            string ImageUrl,
            string Country,
            DateTime DateOfBirth,

            string? Instagram,
            string? Facebook,
            string? GitHub,
            string? LinkedIn,
          


         List<progressBarResponse> Roadmaps_

        );
            

       
    
    
}
