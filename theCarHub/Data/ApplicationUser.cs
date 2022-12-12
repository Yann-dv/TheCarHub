using Microsoft.AspNetCore.Identity;

namespace theCarHub.Data;

public class ApplicationUser : IdentityUser
{

    public ApplicationUser()
    {
        CarList = new HashSet<UserCar>();
    }

    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
    [PersonalData]
    public virtual ICollection<UserCar> CarList { get; set; }
}