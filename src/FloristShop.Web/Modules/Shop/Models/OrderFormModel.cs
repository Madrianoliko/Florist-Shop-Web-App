using System.ComponentModel.DataAnnotations;

namespace FloristShop.Web.Modules.Shop.Models;

/// <summary>
/// Model formularza zamówienia bukietu.
/// Klient wybiera jeden szablon z katalogu + ilość + dane kontaktowe + dostawa.
///
/// MVP: jedno zamówienie = jeden szablon bukietu (ewentualnie kilka sztuk).
/// Jeśli klient chce kilka różnych bukietów, składa oddzielne zamówienia.
/// Upraszcza formularz i logikę SSR do minimum — można rozbudować post-MVP.
/// </summary>
public class OrderFormModel : IValidatableObject
{
    [Required(ErrorMessage = "Wybierz bukiet z katalogu.")]
    public Guid? TemplateId { get; set; }

    [Required(ErrorMessage = "Podaj ilość.")]
    [Range(1, 50, ErrorMessage = "Ilość musi być między 1 a 50.")]
    public int Quantity { get; set; } = 1;

    // === Dane kontaktowe ===
    [Required(ErrorMessage = "Podaj imię i nazwisko.")]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj adres e-mail.")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format e-mail.")]
    [MaxLength(200)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Podaj numer telefonu.")]
    [Phone(ErrorMessage = "Nieprawidłowy format telefonu.")]
    [MaxLength(50)]
    public string CustomerPhone { get; set; } = string.Empty;

    // === Dostawa ===
    [Required(ErrorMessage = "Wybierz sposób dostawy.")]
    public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Pickup;

    /// <summary>Wymagany gdy DeliveryMethod != Pickup. Walidowany przez IValidatableObject.</summary>
    [MaxLength(500)]
    public string? DeliveryAddress { get; set; }

    [Required(ErrorMessage = "Podaj datę realizacji.")]
    public DateOnly DeliveryDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(3));

    // === Notatka klienta ===
    public string? CustomerNote { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Data realizacji: nie w przeszłości
        if (DeliveryDate < DateOnly.FromDateTime(DateTime.Today))
        {
            yield return new ValidationResult(
                "Data realizacji nie może być w przeszłości.",
                [nameof(DeliveryDate)]);
        }

        // Adres dostawy: wymagany dla dostawy lokalnej i kuriera
        if (DeliveryMethod != DeliveryMethod.Pickup &&
            string.IsNullOrWhiteSpace(DeliveryAddress))
        {
            yield return new ValidationResult(
                "Adres dostawy jest wymagany przy wybranej metodzie dostawy.",
                [nameof(DeliveryAddress)]);
        }
    }
}
