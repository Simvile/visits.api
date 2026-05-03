using Microsoft.EntityFrameworkCore;
using visits.api.Data;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.services;

public class InstitutionManager(AppDbContext dbContext): BaseManager<Institution>(dbContext), IInstitutionManager
{
    #region Validate
    public override async Task<ResponseHandler> Validate(Institution entity)
    {
        var response = new ResponseHandler();

        if(string.IsNullOrWhiteSpace(entity.Name))
            response.AddMessage("Institution Code is required");
        
        if(string.IsNullOrWhiteSpace(entity.Code))
            response.AddMessage("Institution Code is required");
        
        if(entity.TypeId == Guid.Empty)
            response.AddMessage("A Valid Institution Type is required");
        
        // Now we need to verify that the Institution Code is Unique
        var search = new SearchObject<Institution>();
        search.Field(x => x.Code).SetValue(entity.Code, SearchType.Equals);
        search.Field(x => x.Id).SetValue(entity.Id, SearchType.NotEquals);
        
        if(await Exists(search))
            response.AddMessage($"Institution Code  '{entity.Code}' already exists");
        
        return response;
    }
    #endregion

    #region GetBYId
    public override async Task<Institution?> GetById(Guid id)
    {
        return await dbContext.Set<Institution>()
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
    }
    #endregion
    
    #region GetForDropdown
    public async Task<List<DropdownModel>> GetForDropdown(string? searchText = null)
    {
        var search = new SearchObject<Institution>();
        search.Field(x => x.IsActive).SetValue(true, SearchType.Equals);

        if (!string.IsNullOrWhiteSpace(searchText))
            search.Field(x => x.Name).SetValue(searchText, SearchType.Contains);

        var list = await GetAll(search);

        return list is null or [] ? [] :
        [
            ..list.Select(x => new DropdownModel
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Name
            }).OrderBy(x => x.Code)
        ];
    }
    #endregion
}