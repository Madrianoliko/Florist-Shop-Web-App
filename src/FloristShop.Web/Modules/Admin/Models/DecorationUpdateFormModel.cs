using FloristShop.Web.Modules.Decoration.Models;

namespace FloristShop.Web.Modules.Admin.Models;

/// <summary>Model formularza aktualizacji zapytania o dekoracje przez admina.</summary>
public class DecorationUpdateFormModel
{
    public DecorationInquiryStatus Status { get; set; }
    public string? AdminNote { get; set; }

    [System.ComponentModel.DataAnnotations.Range(0, 999999)]
    public decimal? FinalPrice { get; set; }
}
