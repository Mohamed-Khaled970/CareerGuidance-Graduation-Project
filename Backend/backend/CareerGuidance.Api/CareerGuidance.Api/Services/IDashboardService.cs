namespace CareerGuidance.Api.Services
{
    public interface IDashboardService
    {
        Task<Result<IEnumerable<UsersResponse>>> GetAllUsers(CancellationToken cancellationToken = default);
        Task<Result<userprofileResponse?>> GetUserById(Guid Id, CancellationToken cancellationToken = default);
        Task<Result> AddNewUserAsync(AddNewUserRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddQuestion(QuestionRequest request , CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionsResponse>>> GetAllQuestions(CancellationToken cancellationToken = default);
        Task<Result> UpdateQuestion(int id ,QuestionRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteQuestion(int id, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserAsync(string id, CancellationToken cancellationToken = default);
        Task<Result> UpdateAdminProfile(UpdateAdminProfileRequest request, CancellationToken cancellationToken = default);

    }
}
