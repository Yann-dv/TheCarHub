namespace theCarHub.Data;

public class CarImages
{
    public virtual Car Car { get; set; }
    
    public int Id { get; set; }
    
    public int CarId { get; set; }
    public string ImageUrl { get; set; }
}