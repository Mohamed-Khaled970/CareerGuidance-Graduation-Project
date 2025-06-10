
using CareerGuidance.Api.Models;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace CareerGuidance.Api.Services
{
    public class progressBarServices : IprogressBarServices
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext __applicationDbContext;

        public progressBarServices(ApplicationDbContext applicationDbContext, IMapper mapper)

        {
            __applicationDbContext = applicationDbContext;
            _mapper = mapper;

        }

      

        public async Task<Result> AddProgressAsync(progressBarRequest request, CancellationToken cancellationToken)
        {
            var user = await __applicationDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                return Result.Failuer(progressBarError.IsEmpty);

            var progress = new progressBar
            {
                Id = request.id != Guid.Empty ? request.id : Guid.NewGuid(),
                ApplicationUserId = request.UserId,
                RoadmapId = Guid.Parse(request.roadmapId),
                RoadmapName = request.roadmapName,
                Progress = request.progressValue,
                CompletedNodes = request.completedNodes
            };

            await __applicationDbContext.progressBar.AddAsync(progress, cancellationToken);
            await __applicationDbContext.SaveChangesAsync(cancellationToken);

            return Result.Success("Progress saved successfully.");
        }





        public async Task<Result<IEnumerable<ProgressBarResponse_2?>>> GetAllProgressAsync(CancellationToken cancellationToken)
        {
            var progressList = await __applicationDbContext.progressBar
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!progressList.Any())
                return Result.Failuer<IEnumerable<ProgressBarResponse_2>>(progressBarError.NoInfoFound);

            var progressBarResponses = progressList
                .Select(p => new ProgressBarResponse_2(
                    p.Id,
                    p.ApplicationUserId,
                    p.RoadmapId.ToString(),
                    p.RoadmapName,
                    p.Progress,
                    p.CompletedNodes
                ))
                .ToList();

            return Result.Success<IEnumerable<ProgressBarResponse_2?>>(progressBarResponses);

        }

    


        public async Task<Result<ProgressBarResponse_2>> GetProgressById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var progress = await __applicationDbContext.progressBar
                .FirstOrDefaultAsync(pb => pb.Id == id, cancellationToken);

            if (progress == null)
            {
                return Result.Failuer<ProgressBarResponse_2>(progressBarError.NoInfoFound);
            }

            var response = new ProgressBarResponse_2(
                Id: progress.Id,
                UserId: progress.ApplicationUserId,
                roadmapId: progress.RoadmapId.ToString(),
                roadmapName: progress.RoadmapName,
                progressValue: progress.Progress,
                completedNodes: progress.CompletedNodes
            );

            return Result.Success(response);
        }




       
        public async Task<Result> UpdateProgressBarAsync(Guid id, progressBarRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await __applicationDbContext.progressBar
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (existing != null)
            {
                existing.ApplicationUserId = request.UserId;
                existing.RoadmapId = Guid.Parse(request.roadmapId);
                existing.RoadmapName = request.roadmapName;
                existing.Progress = request.progressValue;
                existing.CompletedNodes = request.completedNodes;

                // هنا المفروض تستخدم Update عشان تحدث الكيان بدل ما تضيفه تاني
                __applicationDbContext.progressBar.Update(existing);
            }
            else
            {
                var newProgress = new progressBar
                {
                    Id = id,
                    ApplicationUserId = request.UserId,
                    RoadmapId = Guid.Parse(request.roadmapId),
                    RoadmapName = request.roadmapName,
                    Progress = request.progressValue,
                    CompletedNodes = request.completedNodes
                };

                await __applicationDbContext.progressBar.AddAsync(newProgress, cancellationToken);
            }

            await __applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result.Success("Progress updated successfully.");
        }

    }
}






