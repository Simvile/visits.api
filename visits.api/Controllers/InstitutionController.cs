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
    public class InstitutionController(IInstitutionService institutionService) : ControllerBase
    {
        [HttpPost("Save")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseHandler), 200)]
        public async Task<IActionResult> Save(InstitutionMaster institutionMaster)
        {
            return Ok(await institutionService.SaveAsync(institutionMaster));
        }
        
        [HttpGet("GetById/{institutionId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(InstitutionMaster), 200)]
        public async Task<IActionResult> GetById([FromRoute] string institutionId)
        {
            return Ok(await institutionService.GetbyIdAsync(institutionId));
        }
        
        [HttpGet("GetByAll")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<InstitutionMaster>), 200)]
        public async Task<IActionResult> GetByAll()
        {
            return Ok(await institutionService.GetAllAsync());
        }
        
        [HttpGet("GetForDropdown")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<DropdownModel>), 200)]
        public async Task<IActionResult> GetForDropdown(string? searText)
        {
            return Ok(await institutionService.GetForDropdown(searText));
        }
    }
}
