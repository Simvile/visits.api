using visits.api.Data;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.services;

public class AddressManager(AppDbContext dbContext) : BaseManager<Address>(dbContext), IAddressManager
{
    public override async Task<ResponseHandler> Validate(Address address)
    {
        var response = new ResponseHandler();
        
        if(string.IsNullOrWhiteSpace(address.Street))
            response.AddMessage($"Street Is Required for Address");
        
        if (string.IsNullOrWhiteSpace(address.City))
            response.AddMessage($"City Is Required for Address");
        
        var search = new SearchObject<Address>();
        search.Field(s => s.Street).SetValue(address.Street, SearchType.Equals);
        search.Field(s => s.City).SetValue(address.City,  SearchType.Equals);
        search.Field(s => s.Id).SetValue(address.Id, SearchType.NotEquals);

        if (await Exists(search))
        {
            response.AddMessage($"Address Already Exists");
        }
        
        return response;
    }

    public async Task<List<DropdownModel>> GetForDropdown(string? searchText)
    {
        var search = new SearchObject<Address>();
        search.Field(x => x.IsActive).SetValue(true, SearchType.Equals);

        if (!string.IsNullOrWhiteSpace(searchText))
            search.Field(x => x.Street).SetValue(searchText, SearchType.Contains);

        var list = await GetAll(search);

        return list is null or [] ? [] :
        [
            ..list.Select(x => new DropdownModel
            {
                Id = x.Id,
                Code = x.Street,
                Description = x.City
            }).OrderBy(x => x.Code)
        ];
    }
}