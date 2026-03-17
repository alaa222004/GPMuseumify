using System;

namespace GPMuseumify.BL.DTOs.Search;

/// <summary>
/// Lightweight, language-resolved statue details for clients (Flutter, etc.).
/// </summary>
public class StatueDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
}