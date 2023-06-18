namespace BlazorApp.Models;

public class VulnerableDevices
{
    public string? Model { get; set; }
    public string? AffectedFirmware { get; set; }
    public string? FixedFirmware { get; set; }
}