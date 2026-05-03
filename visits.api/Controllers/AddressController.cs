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
    public class AddressController(IAddressService service) : ControllerBase
    {
        [HttpPost("Save")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseHandler), 200)]
        public async Task<IActionResult> Save(AddressMaster institutionMaster)
        {
            return Ok(await service.SaveAsync(institutionMaster));
        }
        
        [HttpGet("GetById/{addressId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AddressMaster), 200)]
        public async Task<IActionResult> GetById([FromRoute] string addressId)
        {
            return Ok(await service.GetByIdAsync(addressId));
        }
        
        [HttpGet("GetByAll")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<AddressMaster>), 200)]
        public async Task<IActionResult> GetByAll()
        {
            return Ok(await service.GetAllAsync());
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
