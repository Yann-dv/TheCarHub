using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace theCarHub.Data
{
    public class Car
    {
        public int Id { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string? Name { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        [BindProperty, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Year { get; set; }
    }
}
