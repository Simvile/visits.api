using visits.api.Configurations;
using visits.api.DTOs;
using visits.api.Interfaces;
using visits.api.Manager.Interfaces;
using visits.api.Utils;
using visits.models.Core;

namespace visits.api.Services;

public class AddressService(IAddressManager manager, IUserContext userContext) : IAddressService
{
    public async Task<AddressMaster?> GetByIdAsync(string id)
    {
        if(!Guid.TryParse(id, out Guid guid) || guid == Guid.Empty)
            throw new ArgumentException($"{nameof(id)} is not a valid Guid");
        
        var coreAddress = await manager.GetById(guid);
        
        if(coreAddress is null)
            return null;

        return new AddressMaster
        {
            Id =  coreAddress.Id,
            Street =  coreAddress.Street,
            City = coreAddress.City,
            Complex =  coreAddress.Complex,
            Province =  coreAddress.Province,
            PostalCode = coreAddress.PostalCode,
            
            IsActive =  coreAddress.IsActive,
            CreatedBy =  coreAddress.CreatedBy,
            CreatedAt =   coreAddress.CreatedAt,
            UpdatedBy =  coreAddress.UpdatedBy,
            UpdatedAt =  coreAddress.UpdatedAt
        };
    }

    public async Task<List<AddressMaster>> GetAllAsync()
    {
        var list = await manager.GetAll();

        return list is null or []
            ? []
            :
            [
                ..list.Select(i => new AddressMaster
                {
                    Id = i.Id,
                    Street = i.Street,
                    City = i.City,
                    Complex = i.Complex,
                    Province = i.Province,
                    PostalCode = i.PostalCode,

                    IsActive = i.IsActive,
                    CreatedBy = i.CreatedBy,
                    CreatedAt = i.CreatedAt,
                    UpdatedBy = i.UpdatedBy,
                    UpdatedAt = i.UpdatedAt
                }).OrderBy(x => x.Street)
            ];
    }

    public async Task<ResponseHandler> SaveAsync(AddressMaster entity)
    {
        var address = await manager.GetById(entity.Id)
                      ?? new Address
                      {
                          Id = entity.Id,
                          CreatedBy = userContext.FullName,
                          CreatedAt = DateTime.UtcNow,
                      };
        
        address.Street = entity.Street;
        address.City = entity.City;
        address.Complex = entity.Complex;
        address.Province = entity.Province;
        address.PostalCode = entity.PostalCode;
        address.IsActive = entity.IsActive;
        address.UpdatedBy = userContext.FullName;
        address.UpdatedAt = DateTime.UtcNow;
        
        // let's run some validations
        var responseHandler = await manager.Validate(address);

        if (responseHandler.HasErrorMessage)
            return responseHandler;
        
        var response = await manager.Save(address);

        if (response.HasErrorMessage) 
            return response;
        
        response.Id = address.Id;
        response.AddMessage("Successfully saved Address", ResponseType.SuccessMessage);

        return response;
    }

    public async Task<List<DropdownModel>> GetForDropdown(string? searchText)
    => await manager.GetForDropdown(searchText);
}