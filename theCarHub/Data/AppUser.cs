namespace theCarHub.Data;

public class AppUser: Microsoft.AspNetCore.Identity.IdentityUser
{
    public AppUser() : base()
    {
        this.Watchlist = new HashSet<UserCar>();
    }

    public string FirstName { get; set; }
    public virtual ICollection<UserCar> Watchlist { get; set; }
}