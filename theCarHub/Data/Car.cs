using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace theCarHub.Data
{
    public class Car
    {
        public int Id { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Make { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Model { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Trim { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public float PurchasePrice{ get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Repairs{ get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public float RepairCost{ get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime LotDate{ get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public float SellingPrice { get; set; }
        
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SaleDate{ get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Description{ get; set; }
        
        public bool ToSale { get; set; }
    }
}
