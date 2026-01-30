

namespace GPMuseumify.BL.DTOs.Settings;

public class UserSettingsDto
{
    public string PreferredLanguage { get; set; } = "en";
    public bool NotificationsEnabled {  get; set; }=true;
}
