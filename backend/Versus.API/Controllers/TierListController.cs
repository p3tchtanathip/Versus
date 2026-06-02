using Microsoft.AspNetCore.Mvc;
using Versus.API.Context;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class TierListController(ITierListService service, ICurrentSession session, ILogger<TierListController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTierList([FromQuery] TierListQueryRequest request)
        {
            logger.LogInformation("Getting tier lists. Page={Page}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var result = await service.GetAllAsync(request);
            return Ok(new ApiResponse<PaginatedList<TierListQueryResponse>>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyTierList([FromQuery] TierListQueryRequest request)
        {
            if (session.SessionId is not Guid sessionId)
            {
                return Unauthorized(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Unauthorized"
                });
            }

            logger.LogInformation("Getting my tier lists. Page={Page}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var result = await service.GetBySessionIdAsync(sessionId, request);
            return Ok(new ApiResponse<PaginatedList<TierListQueryResponse>>
            {
                Success = true,
                Data = result
            });
        }
    }
}