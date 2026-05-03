using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.Interfaces;

public interface IInstitutionManager : IBaseManager<Institution>
{
    Task<List<DropdownModel>> GetForDropdown(string? searchText = null);
}