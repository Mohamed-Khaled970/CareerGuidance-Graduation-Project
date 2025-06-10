
﻿using CareerGuidance.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerGuidance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userProfileController(IuserProfileServices services) : ControllerBase
    {
        private readonly IuserProfileServices _services = services;

        // Retrieves all users
        [HttpGet("GetAllInfo")]
        public async Task<IActionResult> GetAllInfo(CancellationToken cancellationToken)
        {
            var result = await _services.GetAllInfo();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("GetUserById/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var results = await _services.GetUserById(id, cancellationToken);

            return results.IsSuccess ? Ok(results.Value) : results.ToProblem();

        }


        [HttpPut("UpdateProfile/{id:guid}")]

        public async Task<IActionResult> UpdateProfile([FromRoute]Guid id, userprofileRequest request, CancellationToken cancellationToken)

       
        {
            var result = await _services.UpdateUserProfileAsync(id, request, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : result.ToProblem();
        }

        [HttpPut("UpdatePassword/{id:guid}")]

        public async Task<IActionResult> UpdatePassword([FromRoute] Guid id, userPasswordRequest requestpass, CancellationToken cancellationToken)

        {
            var result = await _services.UpdatePassword(id, requestpass, cancellationToken);

            return result.IsSuccess
                ? Ok()
                : result.ToProblem();
        }
    }
}
