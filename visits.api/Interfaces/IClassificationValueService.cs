using visits.api.DTOs;
using visits.api.Utils;

namespace visits.api.Interfaces;

public interface IClassificationValueService
{
    Task<ClassificationValueMaster?> GetByIdAsync(string id);
    Task<List<ClassificationValueMaster>> GetByTypeAsync(string type);
    Task<ResponseHandler> SaveAsync(ClassificationValueMaster entity);
    Task<List<DropdownModel>> GetForDropdown(string? searchText);
}