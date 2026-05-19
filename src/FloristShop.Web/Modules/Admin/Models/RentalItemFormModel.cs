using System.ComponentModel.DataAnnotations;

namespace FloristShop.Web.Modules.Admin.Models;

public class RentalItemFormModel
{
    [Required(ErrorMessage = "Podaj nazwę rekwizytu.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj cenę wypożyczenia.")]
    [Range(0.01, 99999, ErrorMessage = "Cena musi być większa od 0.")]
    public decimal PricePerRental { get; set; }

    [Required(ErrorMessage = "Podaj ilość sztuk.")]
    [Range(1, 9999, ErrorMessage = "Ilość musi wynosić co najmniej 1.")]
    public int TotalQuantity { get; set; } = 1;

    public bool IsActive { get; set; } = true;
}
