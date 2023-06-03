using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class SignInInput
{
    [DisplayName("Mail adresi : ")]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }
    [DisplayName("Parola : ")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [DisplayName("Beni hatırla : ")]
    public bool RememberMe { get; set; }
}