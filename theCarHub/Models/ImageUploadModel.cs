using System.ComponentModel.DataAnnotations;

namespace theCarHub.Models;

public class ImageUploadModel
{
    [DataType(DataType.Upload)]
    [Display(Name = "Upload File")]
    [Required(ErrorMessage = "Please choose file to upload.")]
    public string file { get; set; }
}