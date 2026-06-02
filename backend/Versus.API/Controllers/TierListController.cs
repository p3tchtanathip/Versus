using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Requests;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TierListController(ITierListService service, ILogger<TierListController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTierList([FromQuery] TierListQueryRequest request)
        {
            logger.LogInformation("Getting tier lists. Page={Page}, PageSize={PageSize}", request.PageNumber, request.PageSize);

            var result = await service.GetAllAsync(request);

            return Ok(result);
        }
    }
}