using System.ComponentModel.DataAnnotations;

namespace FloristShop.Web.Modules.Rental.Models;

/// <summary>
/// Model formularza zapytania o rezerwację rekwizytów — WYŁĄCZNIE DTO.
/// Nie jest encją EF, nie ma Id, nie wchodzi do bazy bezpośrednio.
/// Żyje tylko na granicy UI → serwis.
///
/// Implementuje IValidatableObject, żeby DataAnnotationsValidator w Blazor
/// mógł uruchomić cross-field walidację (EndDate >= StartDate, daty w przyszłości).
/// </summary>
public class ReservationFormModel : IValidatableObject
{
    [Required(ErrorMessage = "Data odbioru jest wymagana")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(7));

    [Required(ErrorMessage = "Data zwrotu jest wymagana")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(8));

    [Required(ErrorMessage = "Imię i nazwisko jest wymagane")]
    [MaxLength(200, ErrorMessage = "Imię i nazwisko może mieć maks. 200 znaków")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
    [MaxLength(256, ErrorMessage = "Email może mieć maks. 256 znaków")]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu")]
    [MaxLength(20, ErrorMessage = "Numer telefonu może mieć maks. 20 znaków")]
    public string CustomerPhone { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Notatka może mieć maks. 1000 znaków")]
    public string? CustomerNote { get; set; }

    /// <summary>
    /// Walidacja cross-field — nie da się wyrazić atrybutami.
    /// DataAnnotationsValidator w Blazor wywoła tę metodę automatycznie.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (StartDate < today)
        {
            yield return new ValidationResult(
                "Data odbioru nie może być w przeszłości",
                [nameof(StartDate)]);
        }

        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "Data zwrotu nie może być wcześniejsza niż data odbioru",
                [nameof(EndDate)]);
        }
    }
}
