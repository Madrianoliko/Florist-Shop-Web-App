using System.ComponentModel.DataAnnotations;

namespace FloristShop.Web.Modules.Admin.Models;

public class BouquetTemplateFormModel
{
    [Required(ErrorMessage = "Podaj nazwę bukietu.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Podaj cenę bazową.")]
    [Range(0.01, 99999, ErrorMessage = "Cena musi być większa od 0.")]
    public decimal BasePrice { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
