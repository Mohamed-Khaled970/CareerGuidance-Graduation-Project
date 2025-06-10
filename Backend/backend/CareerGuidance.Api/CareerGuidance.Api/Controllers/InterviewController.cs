using CareerGuidance.Api.Abstractions.Const;
using CareerGuidance.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerGuidance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        public InterviewController(IInterviewService interviewService)
        {
            _interviewService = interviewService;
        }

        // Adds a new user
        [HttpPost("AddInterview")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> AddInterview(AddInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.AddInterviewAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpPut("UpdateInterview")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> UpdateInterview(UpdateInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.UpdateInterviewAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("GetMyInterviews")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> GetMyInterviews(CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.GetInterviewsByInterviewerAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("Delete/{interviewId}")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> Delete([FromRoute] int interviewId, CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.DeleteInterviewAsync(interviewId, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("GetMyApplicants/{interviewId}")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> GetMyApplicants([FromRoute] int interviewId , CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.GetApplicantsForInterviewAsync(interviewId,cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("ScheduleInterview/{interviewId}")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> ApplyInterview([FromRoute] int interviewId, [FromBody] ScheduleInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _interviewService.ScheduleInterviewAsync(interviewId, request, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }

        [HttpPut("RejectApplicant/{interviewId}")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> RejectApplicant([FromRoute] int interviewId, [FromBody] RejectApplicantRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _interviewService.RejectApplicantAsync(interviewId, request, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }

        [HttpPost("Apply")]
        [Authorize]
        public async Task<IActionResult> ApplyInterview([FromForm] ApplyInterviewRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _interviewService.ApplyInterviewAsync(request, cancellationToken);
            return response.IsSuccess ? Ok() : response.ToProblem();
        }

        [HttpGet("GetAllInterviews")]
        public async Task<IActionResult> GetAllInterviews(CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.GetAllInterviewesAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetAcceptedApplicants")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> GetAcceptedApplicants(CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.GetAcceptedApplicantsAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("profile/update")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> UpdateInterviewerProfile(UpdateInterviewerProfileRequest request, CancellationToken cancellationToken = default)
        {

            var result = await _interviewService.UpdateInterviewerProfileAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("profile/{interviewerId}")]
        public async Task<IActionResult> GetInterviewerProfile([FromRoute] string interviewerId , CancellationToken cancellationToken = default)
        {
            var result = await _interviewService.GetInterviewerProfileAsync(interviewerId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("mark-applicant-done/{interviewApplicationId}")]
        [Authorize(Roles = DefaultRole.Instructor)]
        public async Task<IActionResult> MarkApplicantAsDone([FromRoute] int interviewApplicationId, CancellationToken cancellationToken = default)
        {

            var result = await _interviewService.MarkApplicantAsDoneAsync(interviewApplicationId, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
