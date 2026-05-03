using visits.api.Data;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Manager.services;

public class ClassificationManager(AppDbContext dbContext) : BaseManager<ClassificationValues>(dbContext), IClassificationManager
{
    public override async Task<ResponseHandler> Validate(ClassificationValues entity)
    {
        var response = new ResponseHandler();
        
        if(string.IsNullOrWhiteSpace(entity.Code))
            response.AddMessage($"Code Is Required for Classification Values");
        
        if(string.IsNullOrWhiteSpace(entity.Description))
            response.AddMessage($"Description Is Required for Classification Values");
        
        if(string.IsNullOrWhiteSpace(entity.Type))
            response.AddMessage($"Type Is Required for Classification Values");
        
        var search = new SearchObject<ClassificationValues>();
        search.Field(x => x.Code).SetValue(entity.Code, SearchType.Equals);
        search.Field(x => x.Type).SetValue(entity.Type, SearchType.Equals);
        search.Field(x => x.Id).SetValue(entity.Id, SearchType.NotEquals);
        
        if(await Exists(search))
            response.AddMessage($"Code Already Exists for Classification Values {entity.Code}");

        return response;
    }

    public Task<List<ClassificationValues>> GetByType(string type)
    {
        var search = new SearchObject<ClassificationValues>();
        search.Field(x => x.Type).SetValue(type, SearchType.Equals);
        search.Field(x => x.IsActive).SetValue(true, SearchType.Equals);
        
        return GetAll(search);
    }

    public async Task<List<DropdownModel>> GetForDropdown(string? searchText)
    {
        var search = new SearchObject<ClassificationValues>();
        search.Field(x => x.IsActive).SetValue(true, SearchType.Equals);

        if (!string.IsNullOrWhiteSpace(searchText))
            search.Field(x => x.Description).SetValue(searchText, SearchType.Contains);

        var list = await GetAll(search);

        return list is null or [] ? [] :
        [
            ..list.Select(x => new DropdownModel
            {
                Id = x.Id,
                Code = x.Code,
                Description = x.Description,
            }).OrderBy(x => x.Code)
        ];
    }
}