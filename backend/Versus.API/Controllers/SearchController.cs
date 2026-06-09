using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Requests;
using Versus.API.DTOs.Responses;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController(ISearchService service, ILogger<SearchController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchRequest request)
        {
            logger.LogInformation(
                "Searching external source. Category={Category}, Type={Type}, Query={Query}",
                request.Category,
                request.Type,
                request.Query);

            var result = await service.SearchAsync(request);
            return Ok(new ApiResponse<List<SearchResponse>>
            {
                Success = true,
                Data = result
            });
        }
    }
}
