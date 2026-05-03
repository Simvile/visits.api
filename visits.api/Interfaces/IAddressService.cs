using visits.api.DTOs;
using visits.api.Utils;

namespace visits.api.Interfaces;

public interface IAddressService
{
    Task<AddressMaster?> GetByIdAsync(string id);
    Task<List<AddressMaster>> GetAllAsync();
    Task<ResponseHandler> SaveAsync(AddressMaster entity);
    Task<List<DropdownModel>> GetForDropdown(string? searchText);
}