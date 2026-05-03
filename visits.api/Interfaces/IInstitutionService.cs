using visits.api.DTOs;
using visits.api.Utils;

namespace visits.api.Interfaces;

public interface IInstitutionService
{
    Task<ResponseHandler> SaveAsync(InstitutionMaster institutionMaster);
    Task<InstitutionMaster?> GetbyIdAsync(string institutionId);
    Task<List<InstitutionMaster>> GetAllAsync();
    Task<List<DropdownModel>> GetForDropdown(string? searchText = null);
}