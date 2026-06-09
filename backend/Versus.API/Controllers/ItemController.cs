using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Responses;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController(IItemService service, ILogger<ItemController> logger) : ControllerBase
    {
        [HttpGet("{id:guid}/elo-history")]
        public async Task<IActionResult> GetEloHistory([FromRoute] Guid id)
        {
            logger.LogInformation("Getting ELO history. ItemId={ItemId}", id);

            var result = await service.GetEloHistoryAsync(id);
            return Ok(new ApiResponse<ItemEloHistoryResponse>
            {
                Success = true,
                Data = result
            });
        }
    }
}
