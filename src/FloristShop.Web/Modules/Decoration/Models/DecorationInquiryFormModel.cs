using System.ComponentModel.DataAnnotations;

namespace FloristShop.Web.Modules.Decoration.Models;

/// <summary>
/// Model formularza zapytania o wycenę usługi dekoracji. Wyłącznie DTO — nie jest encją EF.
/// Implementuje IValidatableObject dla walidacji cross-field (data wydarzenia w przyszłości).
/// </summary>
public class DecorationInquiryFormModel : IValidatableObject
{
    [Required(ErrorMessage = "Rodzaj wydarzenia jest wymagany")]
    public DecorationEventType EventType { get; set; } = DecorationEventType.Wedding;

    [Required(ErrorMessage = "Data wydarzenia jest wymagana")]
    public DateOnly EventDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddMonths(3));

    [Required(ErrorMessage = "Miejscowość / adres jest wymagany")]
    [MaxLength(300, ErrorMessage = "Lokalizacja może mieć maks. 300 znaków")]
    public string Location { get; set; } = string.Empty;

    [Required(ErrorMessage = "Opis wizji jest wymagany")]
    [MaxLength(3000, ErrorMessage = "Opis może mieć maks. 3000 znaków")]
    public string Description { get; set; } = string.Empty;

    [Range(0, 999999, ErrorMessage = "Budżet musi być liczbą nieujemną")]
    public decimal? BudgetEstimate { get; set; }

    [Required(ErrorMessage = "Imię i nazwisko jest wymagane")]
    [MaxLength(200, ErrorMessage = "Imię i nazwisko może mieć maks. 200 znaków")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
    [MaxLength(256)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
    [MaxLength(20)]
    public string CustomerPhone { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EventDate <= DateOnly.FromDateTime(DateTime.Today))
            yield return new ValidationResult(
                "Data wydarzenia musi być w przyszłości",
                [nameof(EventDate)]);
    }
}
