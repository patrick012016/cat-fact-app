using System.ComponentModel.DataAnnotations;

namespace CatFactApp.Configuration;

public class CatFactConfig
{
    [Required(ErrorMessage = "The API URL is required.")]
    public string ApiUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Request endpoint is required.")]
    public string RequestUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "The output file path is required.")]
    public string OutputFilePath { get; set; } = string.Empty;
}
