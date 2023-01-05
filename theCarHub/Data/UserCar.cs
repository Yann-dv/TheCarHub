namespace theCarHub.Data;

public class UserCar
{
    public virtual ApplicationUser User { get; set; }
    public virtual Car Car { get; set; }
    public string UserId { get; set; }
    public int CarId { get; set; }
    public bool InUserBasket { get; set; }
}