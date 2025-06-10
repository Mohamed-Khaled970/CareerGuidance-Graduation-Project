namespace CareerGuidance.Api.Services
{
    public interface IuserProfileServices
    {

        Task<Result<IEnumerable<userprofileResponse>>> GetAllInfo(CancellationToken cancellationToken = default);
        Task<Result<userprofileResponse?>> GetUserById(Guid Id, CancellationToken cancellationToken = default);
        Task<Result> UpdateUserProfileAsync(Guid id, userprofileRequest request, CancellationToken cancellationToken);
        Task<Result> UpdatePassword(Guid id, userPasswordRequest requestpass, CancellationToken cancellationToken);

    }
}
