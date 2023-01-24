using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace theCarHub.Data;

public class ApplicationUser : IdentityUser
{

    public ApplicationUser()
    {
        CarList = new HashSet<UserCar>();
    }
    
    [Required]
    [PersonalData]
    [DefaultValue("EmptyFirstName")]
    public string FirstName { get; set; }
    
    [Required]
    [PersonalData]
    [DefaultValue("EmptyLastName")]
    public string LastName { get; set; }
    
    [PersonalData]
    public virtual ICollection<UserCar> CarList { get; set; }
}