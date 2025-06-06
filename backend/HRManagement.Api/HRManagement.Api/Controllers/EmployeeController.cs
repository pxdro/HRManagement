using HRManagement.Application.DTOs;
using HRManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HRManagement.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(IEmployeeService service, ILogger<EmployeeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResultDto<IEnumerable<EmployeeReturnDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation($"GET {nameof(GetAll)} called");            
            var result = await _service.GetAllAsync();
            LogResult("GET", nameof(GetAll), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResultDto<EmployeeReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"GET {nameof(GetById)} called");            
            var result = await _service.GetByIdAsync(id);
            LogResult("GET", nameof(GetById), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<EmployeeReturnDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] EmployeeRequestDto dto)
        {
            _logger.LogInformation($"POST {nameof(Create)} called");            
            var result = await _service.AddAsync(dto);
            LogResult("POST", nameof(Create), result);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ResultDto<EmployeeReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResultDto<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeRequestDto dto)
        {
            _logger.LogInformation($"PUT {nameof(Update)} called");            
            var result = await _service.UpdateAsync(id, dto);
            LogResult("PUT", nameof(Update), result);
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
            LogResult<bool>("DELETE", nameof(Delete), result);
            return StatusCode((int)result.StatusCode, result);
        }

        private void LogResult<T>(string method, string action, ResultDto<T> result)
        {
            if (result.StatusCode == HttpStatusCode.InternalServerError)
                _logger.LogError("{Method} {Action} internal error with message {ErrorMessage}", method, action, result.ErrorMessage);
            else if (result.IsSuccess)
                _logger.LogInformation("{Method} {Action} succeeded with code {StatusCode} and data {@Data}", method, action, (int)result.StatusCode, result.Data);
            else
                _logger.LogInformation("{Method} {Action} failed with code {StatusCode} and message {ErrorMessage}", method, action, (int)result.StatusCode, result.ErrorMessage);
        }
    }
}
