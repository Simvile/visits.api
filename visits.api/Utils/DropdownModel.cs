namespace visits.api.Utils;

public class DropdownModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    public string Display => (string.IsNullOrWhiteSpace(Name), string.IsNullOrWhiteSpace(Description)) switch
    {
        (true, _)     => Description ?? string.Empty,
        (false, true) => Name!,
        _             => $"{Name} - {Description}"
    };
}