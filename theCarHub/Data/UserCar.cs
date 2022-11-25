namespace theCarHub.Data;

public class UserCar
{
    public virtual AppUser User { get; set; }
    public virtual Car Car { get; set; }
    public string UserId { get; set; }
    public int CarId { get; set; }
    public bool Watched { get; set; }
    public int Rating { get; set; }
}