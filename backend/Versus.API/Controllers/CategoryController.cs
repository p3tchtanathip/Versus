using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Responses;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController(ICategoryService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await service.GetAllAsync();
            return Ok(new ApiResponse<List<CategoryResponse>>
            {
                Success = true,
                Data = result
            });
        }
    }
}