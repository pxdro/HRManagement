using HRManagement.Api.Shared;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ResultDto<TokensDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            _logger.LogInformation($"POST {nameof(Login)} called");
            var result = await _service.LoginAsync(loginDto);
            _logger.LogResult("POST", "/api/auth/login", result);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
