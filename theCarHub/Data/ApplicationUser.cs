﻿namespace theCarHub.Data;

public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
{

    public ApplicationUser() : base()
    {
        this.CarList = new HashSet<UserCar>();
    }

    public string FirstName { get; set; }
    public virtual ICollection<UserCar> CarList { get; set; }
}