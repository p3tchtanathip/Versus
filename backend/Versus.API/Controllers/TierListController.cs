using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Models.Common;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/lists")]
    [ApiController]
    public class TierListController(
        ITierListService tierListService,
        IItemService itemService,
        IMatchService matchService,
        ILogger<TierListController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTierList([FromQuery] TierListQueryRequest request)
        {
            logger.LogInformation("Getting tier lists. Page={Page}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var result = await tierListService.GetAllAsync(request);
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

            var result = await tierListService.GetMeAsync(request);
            return Ok(new ApiResponse<PaginatedList<TierListResponse>>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTierList([FromRoute] Guid id)
        {
            var result = await tierListService.GetByIdAsync(id);
            return Ok(new ApiResponse<TierListResponse>
            {
                Success = true,
                Data = result
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTierList([FromBody] CreateTierListRequest request)
        {
            logger.LogInformation(
                "Creating tier list. Name={Name}, CategoryId={CategoryId}, ItemCount={ItemCount}",
                request.Name,
                request.CategoryId,
                request.Items.Count);

            var result = await tierListService.CreateAsync(request);
            return Ok(new ApiResponse<TierListResponse>
            {
                Success = true,
                Data = result
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTierList([FromRoute] Guid id)
        {
            logger.LogInformation("Deleting tier list. Id={Id}", id);

            await tierListService.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }

        [HttpPost("{id:guid}/items")]
        public async Task<IActionResult> AddItem([FromRoute] Guid id, [FromBody] CreateTierListItemRequest request)
        {
            logger.LogInformation("Adding item to tier list. TierListId={TierListId}, Name={Name}", id, request.Name);

            var result = await itemService.AddAsync(id, request);
            return Ok(new ApiResponse<ItemResponse>
            {
                Success = true,
                Data = result
            });
        }

        [HttpDelete("{id:guid}/items/{itemId:guid}")]
        public async Task<IActionResult> DeleteItem([FromRoute] Guid id, [FromRoute] Guid itemId)
        {
            logger.LogInformation("Deleting item from tier list. TierListId={TierListId}, ItemId={ItemId}", id, itemId);

            await itemService.DeleteAsync(id, itemId);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }

        [HttpGet("{id:guid}/next-match")]
        public async Task<IActionResult> GetNextMatch([FromRoute] Guid id)
        {
            logger.LogInformation("Getting next match. TierListId={TierListId}", id);

            var result = await matchService.GetNextMatchAsync(id);
            return Ok(new ApiResponse<NextMatchResponse>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("{id:guid}/results")]
        public async Task<IActionResult> GetResults([FromRoute] Guid id)
        {
            logger.LogInformation("Getting tier list results. TierListId={TierListId}", id);

            var result = await tierListService.GetResultsAsync(id);
            return Ok(new ApiResponse<TierListResultsResponse>
            {
                Success = true,
                Data = result
            });
        }

        [HttpGet("{id:guid}/history")]
        public async Task<IActionResult> GetHistory([FromRoute] Guid id)
        {
            logger.LogInformation("Getting match history. TierListId={TierListId}", id);

            var result = await matchService.GetHistoryAsync(id);
            return Ok(new ApiResponse<List<MatchResponse>>
            {
                Success = true,
                Data = result
            });
        }
    }
}