using visits.api.Utils;
using visits.models.Base;

namespace visits.api.DTOs;

public class InstitutionMaster : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public DropdownModel Type { get; set; } = null!;
    public DropdownModel Address { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;
}