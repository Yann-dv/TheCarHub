namespace theCarHub.Data;

public class AppUser: Microsoft.AspNetCore.Identity.IdentityUser
{
    public string FirstName { get; set; }
    public virtual ICollection<UserCar> Watchlist { get; set; }
    public AppUser() : base()
    {
        this.Watchlist = new HashSet<UserCar>();
    }
    }