

using System.ComponentModel.DataAnnotations;

namespace GPMuseumify.BL.DTOs.Auth;

public class DeleteAccountDto
{

    [Required(ErrorMessage = "Password is required to confirm account deletion")]
    public string Password { get; set; } = string.Empty;
}
