using visits.api.DTOs;
using visits.api.Manager.Interfaces;
using visits.api.Manager.services;

namespace visits.api.Manager;

public static class ManagerServiceCollection
{
    public static IServiceCollection AddManagerConfigurations(this IServiceCollection services)
    {
        // Here we add all manager configs we have designed on the system.
        services.AddScoped<IInstitutionManager, InstitutionManager>();
        services.AddScoped<IAddressManager, AddressManager>();
        services.AddScoped<IClassificationManager, ClassificationManager>();
        
        return services;
    }
}