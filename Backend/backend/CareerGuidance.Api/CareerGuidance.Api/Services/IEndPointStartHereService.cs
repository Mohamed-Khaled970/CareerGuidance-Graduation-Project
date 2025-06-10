namespace CareerGuidance.Api.Services
{
    public interface IEndPointStartHereService
    {
        Task<Result<IntroductionStartHerePage>> AddIntroductionStartHereAsync(IntroductionStartHerePageDto  introductionInStartHerePageDto, CancellationToken cancellationToken = default);
        Task<Result> DeleteIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default);
        Task<Result> UpdateIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, IntroductionStartHerePageDto  introductionInStartHerePageDto, CancellationToken cancellationToken = default);
        Task<Result<IntroductionStartHerePage>> GetIntroductionInStartHereAsync(IntroductionStartHerePageId introductionInStartHerePageId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<IntroductionStartHerePage>>> GetAllIntroductionInStartHereAsync(CancellationToken cancellationToken = default);

         

        Task<Result<ImportantStartHere>> AddImportantStartHereAsync(ImportantStartHereDto  importantStartHereDto, CancellationToken cancellationToken = default);
        Task<Result> DeleteImportantStartHereAsync(ImportantStartHereId  importantStartHereId, CancellationToken cancellationToken = default);
        Task<Result>UpdateImportantStartHereAsync(ImportantStartHereId  importantStartHereId, ImportantStartHereDto  importantStartHereDto, CancellationToken cancellationToken = default);
        Task<Result<ImportantStartHere>> GetImportantStartHereAsync(ImportantStartHereId  importantStartHereId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<ImportantStartHere>>> GetAllImportantStartHereAsync(CancellationToken cancellationToken = default);




        Task<Result<NewCarouselSection>>AddCarouselSectionAsync(NewCarouselSectionDto  sectionTypeRoadmapDto, CancellationToken cancellationToken = default);
        Task<Result> DeleteCarouselSectionAsync(NewCarouselSectionId  sectionTypeRoadmapId, CancellationToken cancellationToken = default);
        Task<Result> UpdateCarouselSectionAsync(NewCarouselSectionId sectionTypeRoadmapId, NewCarouselSectionDto sectionTypeRoadmapDto, CancellationToken cancellationToken = default);
        Task<Result<NewCarouselSection>> GetCarouselSectionAsync(NewCarouselSectionId sectionTypeRoadmapId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<NewCarouselSection>>> GetAllCarouselSectionAsync(CancellationToken cancellationToken = default);




        Task<Result<DetailsCarouselSection>> AddDetailsCarouselSectionAsync(DetailsCarouselSectionDto  detailsForCarousel, CancellationToken cancellationToken = default);
        Task<Result> DeleteDetailsCarouselSectionAsync(DetailsCarouselSectionId  detailsForCarouselId, CancellationToken cancellationToken = default);
        Task<Result> UpdateDetailsCarouselSectionAsync(DetailsCarouselSectionId detailsForCarouselId, DetailsCarouselSectionDto  detailsForCarouselDto, CancellationToken cancellationToken = default);
        Task<Result<DetailsCarouselSection>> GetDetailsCarouselSectionAsync(DetailsCarouselSectionId detailsForCarouselId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<DetailsCarouselSection>>> GetAllDetailsCarouselSectionAsync(CancellationToken cancellationToken = default);

    } 
}
