namespace visits.api.Utils;

public class DropdownModel
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }

    public string Display => (string.IsNullOrWhiteSpace(Code), string.IsNullOrWhiteSpace(Description)) switch
    {
        (true, _)     => Description ?? string.Empty,
        (false, true) => Code!,
        _             => $"{Code} - {Description}"
    };
}