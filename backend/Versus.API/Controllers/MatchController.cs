using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/matches")]
    [ApiController]
    public class MatchController(IMatchService service, ILogger<MatchController> logger) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
        {
            logger.LogInformation(
                "Recording match. TierListId={TierListId}, WinnerId={WinnerId}, LoserId={LoserId}",
                request.TierListId,
                request.WinnerId,
                request.LoserId);

            var result = await service.CreateAsync(request);
            return Ok(new ApiResponse<MatchResponse>
            {
                Success = true,
                Data = result
            });
        }
    }
}
