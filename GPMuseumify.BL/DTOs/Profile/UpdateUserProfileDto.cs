

using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Profile;

public class UpdateUserProfileDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Phone]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
}
