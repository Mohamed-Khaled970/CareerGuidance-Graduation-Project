namespace CareerGuidance.Api.Errors
{
    public class EndPointHomePageError
    {
        public static readonly Error InvalidData =
    new("IntroductionHomePage.InvalidData", "Invalid Data", StatusCodes.Status400BadRequest);

        public static readonly Error DescriptionDuplicated =
            new("IntroductionHomePage.DescriptionDuplicated", "This description already exists", StatusCodes.Status409Conflict);

        public static readonly Error NotFound =
            new("IntroductionHomePage.NotFound", "The requested resource was not found", StatusCodes.Status404NotFound);

        public static readonly Error FieldRequired =
            new("IntroductionHomePage.FieldRequired", "This field is required and cannot be empty", StatusCodes.Status400BadRequest);

    }
}
