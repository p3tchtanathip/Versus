using Microsoft.AspNetCore.Mvc;
using Versus.API.DTOs.Responses;
using Versus.API.Services.Interfaces;

namespace Versus.API.Controllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionController(ISessionService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateSession()
        {
            var result = await service.CreateAsync();
            return Ok(new ApiResponse<Guid>
            {
                Success = true,
                Data = result
            });
        }
    }
}