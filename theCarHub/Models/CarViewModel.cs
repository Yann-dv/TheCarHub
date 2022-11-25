namespace theCarHub.Models;

public class CarViewModel
{
    public int CarId { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public bool InWatchlist { get; set; }
    public bool Watched { get; set; }
    public int? Rating { get; set; }
}