namespace CareerGuidance.Api.Errors
{
    public class EndPointStartHereError
    {
        public static readonly Error InvalidIntroductionData =
            new("StartHere.InvalidIntroductionData", "Invalid introduction data ", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicateIntroductionTitle =
            new("StartHere.DuplicateIntroductionTitle", "The introduction title already exists", StatusCodes.Status409Conflict);

        public static readonly Error DuplicateIntroductionDescription =
            new("StartHere.DuplicateIntroductionDescription", "The introduction description already exists", StatusCodes.Status409Conflict);

        public static readonly Error FieldRequired =
            new("StartHere.FieldRequired", "This field is required and cannot be empty", StatusCodes.Status400BadRequest);

        public static readonly Error NotFound =
            new("StartHere.NotFound", "The requested resource was not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidImportantData =
            new("StartHere.InvalidImportantData", "Invalid important data ", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicateImportantTitle =
            new("StartHere.DuplicateImportantTitle", "The important title already exist.", StatusCodes.Status409Conflict);

        public static readonly Error InvalidCarouselSectionData =
            new("StartHere.InvalidSectionTypeData", "Invalid CarouselSection  ", StatusCodes.Status400BadRequest);

        public static readonly Error DuplicateCarouselSection =
            new("StartHere.DuplicateSectionType", "The CarouselSection already exists", StatusCodes.Status409Conflict);


        public static readonly Error DuplicateCarouselData =
            new("StartHere.DuplicateCarouselData", "Duplicate carousel data detected.", StatusCodes.Status409Conflict);

        public static readonly Error DuplicateCarouselTitle =
            new("StartHere.DuplicateCarouselTitle", "The carousel title already exists.", StatusCodes.Status409Conflict);

        public static readonly Error SectionNotFound =
            new("StartHere.SectionNotFound", "The specified section does not exist.", StatusCodes.Status404NotFound);

        // New errors added
        public static readonly Error InvalidCarouselDescription =
            new("StartHere.InvalidCarouselDescription", "Invalid Carousel description data", StatusCodes.Status400BadRequest);
        
        public static readonly Error InvalidCarouselTitle =
            new("StartHere.InvalidCarouselTitle", "Invalid Carousel Title data", StatusCodes.Status400BadRequest);
        
        public static readonly Error InvalidCarouselState =
            new("StartHere.InvalidCarouselState", "Invalid Carousel State data", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidCarouselImg =
            new("StartHere.InvalidCarouselImg", "Invalid carousel image ", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidCarouselUrl = new( "StartHere.InvalidCarouselUrl",
    "The carousel URL must be a valid link starting with http or https.", StatusCodes.Status400BadRequest);

        // Limiting the length of specific fields errors
        public static readonly Error CarouselSectionLength =
            new("StartHere.CarouselSectionLength", "carouselSection must be at least 3 characters", StatusCodes.Status400BadRequest);

        public static readonly Error CarouselTitleLength =
            new("StartHere.CarouselTitleLength", "carouselTitle must be at least 3 characters", StatusCodes.Status400BadRequest);

        public static readonly Error CarouselStateLength =
            new("StartHere.CarouselStateLength", "carouselState must be at least 3 characters", StatusCodes.Status400BadRequest);

        public static readonly Error CarouselDescriptionLength =
            new("StartHere.CarouselDescriptionLength", "carouselDes must be at least 3 characters", StatusCodes.Status400BadRequest);

        public static readonly Error CarouselImgLength =
            new("StartHere.CarouselImgLength", "carouselImg must be at least 3 characters", StatusCodes.Status400BadRequest);

    }
}
