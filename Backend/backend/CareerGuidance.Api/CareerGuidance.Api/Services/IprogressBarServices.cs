namespace CareerGuidance.Api.Services
{
    public interface IprogressBarServices
    {
        Task<Result<IEnumerable<ProgressBarResponse_2?>>> GetAllProgressAsync(CancellationToken cancellationToken = default);
        Task<Result<ProgressBarResponse_2?>> GetProgressById(Guid Id, CancellationToken cancellationToken = default);
        Task<Result> AddProgressAsync(progressBarRequest request, CancellationToken cancellationToken);
        Task<Result> UpdateProgressBarAsync(Guid id ,progressBarRequest request ,CancellationToken cancellationToken = default);

     
    }
}
