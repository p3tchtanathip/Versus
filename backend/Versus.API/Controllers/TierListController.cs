using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class TierListController(ITierListService service, ILogger<TierListController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTierList([FromQuery] TierListQueryRequest request)
        {
            logger.LogInformation("Getting tier lists. Page={Page}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var result = await service.GetAllAsync(request);
            return Ok(new ApiResponse<PaginatedList<TierListResponse>>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyTierList([FromQuery] TierListQueryRequest request)
        {
            logger.LogInformation("Getting my tier lists. Page={Page}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var result = await service.GetMeAsync(request);
            return Ok(new ApiResponse<PaginatedList<TierListResponse>>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTierList([FromRoute] Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return Ok(new ApiResponse<TierListResponse>
            {
                Success = true,
                Data = result
            });
        }
    }
}