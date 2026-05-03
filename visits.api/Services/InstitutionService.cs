using visits.api.Configurations;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Services;

public class InstitutionService(IInstitutionManager manager, IUserContext userContext, IAddressManager addressManager, IClassificationManager classificationManager) : IInstitutionService
{
    #region SaveAsync
    public async Task<ResponseHandler> SaveAsync(InstitutionMaster institutionMaster)
    {
        // let's try to get the core object
        var coreObject = await manager.GetById(institutionMaster.Id)
                         ?? new Institution
                         {
                             Id = institutionMaster.Id,
                             CreatedBy = userContext.FullName,
                             CreatedAt = DateTime.UtcNow
                         };
        
        // map the rest of the properties
        coreObject.Name = institutionMaster.Name;
        coreObject.Code = institutionMaster.Code;
        coreObject.TypeId = institutionMaster.Type.Id;
        coreObject.AddressId = institutionMaster.Address.Id;
        coreObject.LogoUrl = institutionMaster.LogoUrl;
        
        coreObject.IsActive = institutionMaster.IsActive;
        coreObject.UpdatedBy = userContext.FullName;
        coreObject.UpdatedAt = DateTime.UtcNow;
        
        // let's run some validations
        var responseHandler = await manager.Validate(coreObject);

        if (responseHandler.HasErrorMessage)
            return responseHandler;
        
        // save object
        responseHandler = await manager.Save(coreObject);

        if (responseHandler.HasErrorMessage) 
            return responseHandler;
        
        responseHandler.Id = coreObject.Id;
        responseHandler.AddMessage($"Successfully saved institution {coreObject.Name}", ResponseType.SuccessMessage);

        return responseHandler;
    }
    #endregion
    
    #region GetbyIdAsync
    public async Task<InstitutionMaster?> GetbyIdAsync(string institutionId)
    {
        // verify the ID value first
        if (!Guid.TryParse(institutionId, out var guid))
            return null;

        var coreObj = await manager.GetById(guid);
        
        if(coreObj is null)
            return null;
        
        var address = await addressManager.GetById(coreObj.AddressId)
            ?? throw new NullReferenceException($"Address with id {coreObj.AddressId} does not exist");
        
        var type = await classificationManager.GetById(coreObj.TypeId)
            ?? throw new NullReferenceException("Institution Type not found");
        
        // map to dto
        return new InstitutionMaster
        {
            Id          = coreObj.Id,
            Name        = coreObj.Name,
            Code        = coreObj.Code,
            LogoUrl     = coreObj.LogoUrl,
            
            IsActive    = coreObj.IsActive,
            CreatedBy   = coreObj.CreatedBy,
            CreatedAt   = coreObj.CreatedAt,
            UpdatedBy   = coreObj.UpdatedBy,
            UpdatedAt   = coreObj.UpdatedAt,

            Type = new DropdownModel
            {
                Id          = type.Id,
                Code        = type.Code,
                Description = type.Description
            },

            Address = new DropdownModel
            {
                Id          = address.Id,
                Code        = address.Street,
                Description = address.City
            }
        };
    }
    #endregion
    
    #region GetAll
    public async Task<List<InstitutionMaster>> GetAllAsync()
    {
        var list = await manager.GetAll();

        return list is null or []
            ? []
            :
            [
                ..list.Select(i => new InstitutionMaster
                {
                    Id = i.Id,
                    Name = i.Name,
                    Code = i.Code,
                    LogoUrl =  i.LogoUrl,

                    IsActive = i.IsActive,
                    CreatedBy = i.CreatedBy,
                    CreatedAt = i.CreatedAt,
                    UpdatedBy = i.UpdatedBy,
                    UpdatedAt = i.UpdatedAt
                }).OrderBy(x => x.Name)
            ];
    }
    #endregion
    
    #region GetForDropdown
    public async Task<List<DropdownModel>> GetForDropdown(string? searchText = null)
        => await manager.GetForDropdown(searchText);
    
    #endregion
}