using visits.api.Configurations;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Services;

public class ClassificationValueService(IClassificationManager manager, IUserContext userContext) : IClassificationValueService
{
    public async Task<ClassificationValueMaster?> GetByIdAsync(string id)
    {
        if(!Guid.TryParse(id, out Guid guid) || guid == Guid.Empty)
            throw new ArgumentException($"{nameof(id)} is not a valid Guid");
        
        var coreAddress = await manager.GetById(guid);

        return coreAddress is null ? null 
            : new ClassificationValueMaster
        {
            Id =  coreAddress.Id,
            Type =  coreAddress.Type,
            Code =  coreAddress.Code,
            Description =  coreAddress.Description,
            
            IsActive =  coreAddress.IsActive,
            CreatedBy =  coreAddress.CreatedBy,
            CreatedAt =   coreAddress.CreatedAt,
            UpdatedBy =  coreAddress.UpdatedBy,
            UpdatedAt =  coreAddress.UpdatedAt
        };
    }

    public async Task<List<ClassificationValueMaster>> GetByTypeAsync(string type)
    {
        var list = await manager.GetByType(type);

        return list is null or []
            ? []
            : [..list.Select(c => new ClassificationValueMaster
            {
                Id =  c.Id,
                Type =  c.Type,
                Code =  c.Code,
                Description =  c.Description,
                
                IsActive =  c.IsActive,
                CreatedBy =  c.CreatedBy,
                CreatedAt =  c.CreatedAt,
                UpdatedBy =  c.UpdatedBy,
                UpdatedAt =  c.UpdatedAt
            }).OrderBy(x => x.Code)];
    }

    public async Task<ResponseHandler> SaveAsync(ClassificationValueMaster entity)
    {
        var classificationValue = await manager.GetById(entity.Id)
                      ?? new ClassificationValues
                      {
                          Id = entity.Id,
                          CreatedBy = userContext.FullName,
                          CreatedAt = DateTime.UtcNow,
                      };
        
        classificationValue.Type =  entity.Type;
        classificationValue.Code =  entity.Code;
        classificationValue.Description = entity.Description;
        
        classificationValue.IsActive = entity.IsActive;
        classificationValue.UpdatedBy = userContext.FullName;
        classificationValue.UpdatedAt = DateTime.UtcNow;
        
        // let's run some validations
        var responseHandler = await manager.Validate(classificationValue);

        if (responseHandler.HasErrorMessage)
            return responseHandler;
        
        var response = await manager.Save(classificationValue);
        
        if(response.HasErrorMessage)
            return response;
            
        response.Id = classificationValue.Id;
        response.AddMessage("Successfully saved Classification Value", ResponseType.SuccessMessage);
        
        return response;
    }

    public async Task<List<DropdownModel>> GetForDropdown(string? searchText) 
        => await manager.GetForDropdown(searchText);
}