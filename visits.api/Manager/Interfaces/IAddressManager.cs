using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.Interfaces;

public interface IAddressManager: IBaseManager<Address>
{
    Task<List<DropdownModel>> GetForDropdown(string? searchText);
}