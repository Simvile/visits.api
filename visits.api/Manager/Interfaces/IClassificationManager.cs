using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.Interfaces;

public interface IClassificationManager : IBaseManager<ClassificationValues>
{
    Task<List<ClassificationValues>> GetByType(string type);
    Task<List<DropdownModel>> GetForDropdown(string? searchText);
}