
using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Settings;

public class UpdateUserSettingsDto
{
    [MaxLength(5)]
    [RegularExpression("^(en|ar)$ " , ErrorMessage = "PreferredLanguage must be 'en' or 'ar'.")]
    public string? PreferredLanguage { get; set; }
    public bool? NotificationsEnabled { get; set; }
}
