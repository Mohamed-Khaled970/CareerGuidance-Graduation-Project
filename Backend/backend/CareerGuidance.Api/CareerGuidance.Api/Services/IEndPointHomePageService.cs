namespace CareerGuidance.Api.Services
{
    public interface IEndPointHomePageService
    {
        Task<Result<roadmapCategory>> AddRoadmapCategoryAsync(roadmapCategoryDto roadmapCategory, CancellationToken cancellationToken = default);
        Task<Result> UpdateRoadmapCategoryAsync(CategoryIdDto categoryId, roadmapCategoryDto roadmapCategory, CancellationToken cancellationToken = default);
        Task<Result> DeleteRoadmapCategoryAsync(CategoryIdDto categoryId, CancellationToken cancellationToken = default);
        Task<Result<GetCategoryResponse>> GetCategory(CategoryIdDto categoryId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<roadmapCategory>>> GetAllRoadmapCategoryAsync(CancellationToken cancellationToken = default);


        /*
        Task<InformationHomePageForRoadmap> AddInformationForRoadmapAsync(InformationHomePageForRoadmapDto introductionInHome, CancellationToken cancellationToken = default);
        Task<bool> UpdateInformationForRoadmapAsync(InformationHomePageForRoadmapId  informationHomePageForRoadmapId, InformationHomePageForRoadmapDto introductionInHome, CancellationToken cancellationToken = default);
        Task<bool> DeleteInformationForRoadmapAsync(InformationHomePageForRoadmapId  informationHomePageForRoadmapId, CancellationToken cancellationToken = default);
        Task<InformationHomePageForRoadmap> GetInformationForRoadmapAsync(InformationHomePageForRoadmapId  informationHomePageForRoadmapId, CancellationToken cancellationToken = default);
        Task<IEnumerable<InformationHomePageForRoadmap>> GetAllInformationForRoadmapAsync(CancellationToken cancellationToken = default);
        */


        Task<Result<IntroductionHomePage>> AddIntroductionHomePageAsync(IntroductionHomePageDto introductionHomePageDto, CancellationToken cancellationToken = default);
        Task<Result> DeleteIntroductionHomePageAsync(IntroductionHomePageId introductionHomePageId, CancellationToken cancellationToken = default);
        Task<Result> UpdateIntroductionHomePageAsync(IntroductionHomePageId introductionHomePageId, IntroductionHomePageDto introductionHomePageDto, CancellationToken cancellationToken = default);
        Task<Result<IntroductionHomePage>> GetIntroductionHomePageAsync(IntroductionHomePageId introductionHomePageId , CancellationToken cancellationToken = default);   
        Task<Result<IEnumerable<IntroductionHomePage>>> GetAllIntroductionHomePageAsync(CancellationToken cancellationToken = default);

    }
}
