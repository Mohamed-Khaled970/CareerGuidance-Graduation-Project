using System.Threading;

namespace CareerGuidance.Api.Services
{
    /*
  * File Name: IAllRoadmapsInsertedService.cs
  * Author Information: Abdelrahman Rezk
  * Date of creation: 2024-10-13
  * Versions Information: v1.0
  * Dependencies: DashboardController.cs
  * Contributors: Abdelrahman Rezk
  * Last Modified Date: 2024-10-27
  *
  * Description:
  *      Signature for functions 
  */
    public interface IAllRoadmapsInsertedService
    {
        Task<Result> AddFilteredRoadmapAsync(allRoadmapInsertedDto request, CancellationToken cancellationToken = default);
        Task<IEnumerable<allRoadmapInserted>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result> UpdateRoadmapInformationAsync([FromBody] UpdateRoadmapRequest request, CancellationToken cancellationToken = default);
        Task<allRoadmapInserted?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(Guid sharedId, allRoadmapInsertedDto roadmap_, CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> CheckRoadmapInformationAsync(CreateRoadmapRequest request, CancellationToken cancellationToken = default);




    }
}
