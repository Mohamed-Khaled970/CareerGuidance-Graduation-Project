using CareerGuidance.Api.Abstractions.Const;
using CareerGuidance.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareerGuidance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class progressBarController(IprogressBarServices progressService) : ControllerBase
    {
        private readonly IprogressBarServices _progressService = progressService;


        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var result = await _progressService.GetAllProgressAsync(cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        [HttpGet("GetById/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var results = await _progressService.GetProgressById(id, cancellationToken);
            return results.IsSuccess ? Ok(results.Value) : results.ToProblem();
        }

        [HttpPost("AddProgress")]
        public async Task<IActionResult> AddProgress([FromBody] progressBarRequest request, CancellationToken cancellationToken)
        {
            var result = await _progressService.AddProgressAsync(request, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    


    [HttpPut("UpdateProgressBar{id:guid}")]
        public async Task<IActionResult> UpdateProgressBar(Guid id,[FromBody] progressBarRequest request, CancellationToken cancellationToken)
        {
                var result = await _progressService.UpdateProgressBarAsync(id, request, cancellationToken);
                return result.IsSuccess ? NoContent() : result.ToProblem();
        }

    }
}
