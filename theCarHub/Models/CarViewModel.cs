using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace theCarHub.Models;

public class CarViewModel
{
    public int CarId { get; set; }
    
    public string OwnerId { get; set; }
    
    [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime Year { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    
    public string Trim { get; set; }
    
    [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime PurchaseDate { get; set; }
    
    public decimal PurchasePrice { get; set; }
    
    public string Repairs { get; set; }
    
    public decimal RepairCost { get; set; }

    [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime LotDate { get; set; }
    
    public decimal SellingPrice { get; set; }
    
    [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
    public DateTime SaleDate { get; set; }

    public string Description { get; set; }
    
    public bool ToSale { get; set; }
}