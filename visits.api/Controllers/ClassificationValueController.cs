using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Utils;

namespace visits.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassificationValueController(IClassificationValueService service) : ControllerBase
    {
        [HttpPost("Save")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseHandler), 200)]
        public async Task<IActionResult> Save(ClassificationValueMaster institutionMaster)
        {
            return Ok(await service.SaveAsync(institutionMaster));
        }
        
        [HttpGet("GetById/{classificationValueId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClassificationValueMaster), 200)]
        public async Task<IActionResult> GetById([FromRoute] string classificationValueId)
        {
            return Ok(await service.GetByIdAsync(classificationValueId));
        }
        
        [HttpGet("GetByType/{type}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClassificationValueMaster), 200)]
        public async Task<IActionResult> GetByType([FromRoute] string type)
        {
            return Ok(await service.GetByTypeAsync(type));
        }
        
        [HttpGet("GetForDropdown")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DropdownModel>), 200)]
        public async Task<IActionResult> GetForDropdown(string? searText)
        {
            return Ok(await service.GetForDropdown(searText));
        }
    }
}
