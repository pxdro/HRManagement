using HRManagement.Api.Shared;
using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly ILogger<DepartmentController> _logger;
        public DepartmentController(IDepartmentService service, ILogger<DepartmentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultDto<IEnumerable<DepartmentReturnDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"GET {nameof(GetAll)} called");
            var result = await _service.GetAllAsync();
            _logger.LogResult("GET", nameof(GetAll), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultDto<DepartmentReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET {nameof(GetById)} called");
            var result = await _service.GetByIdAsync(id);
            _logger.LogResult("GET", nameof(GetById), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<DepartmentReturnDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] DepartmentCreationRequestDto dto)
        {
            _logger.LogInformation($"POST {nameof(Create)} called");
            var result = await _service.AddAsync(dto);
            _logger.LogResult("POST", nameof(Create), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResultDto<DepartmentReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentUpdateRequestDto dto)
        {
            _logger.LogInformation($"PUT {nameof(Update)} called");
            var result = await _service.UpdateAsync(id, dto);
            _logger.LogResult("PUT", nameof(Update), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultDto<bool>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DELETE {nameof(Delete)} called");
            var result = await _service.DeleteAsync(id);
            _logger.LogResult("DELETE", nameof(Delete), result);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
