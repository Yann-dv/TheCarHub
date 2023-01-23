using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using theCarHub.Data;

namespace theCarHub.Data
{
    public class Car
    {
        public int Id { get; set; }
        
        public string OwnerId { get; set; }
        
        [Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }
        
        [Required]
        public string Make { get; set; }
        
        [Required]
        public string Model { get; set; }
        
        [Required]
        public string Trim { get; set; }
        
        [Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice{ get; set; }
        
        [Required]
        public string Repairs{ get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RepairCost{ get; set; }
        
        [Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LotDate{ get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellingPrice { get; set; }
        
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SaleDate{ get; set; }
        
        [Required]
        public string Description{ get; set; }
        
        public bool ToSale { get; set; }
    }
}