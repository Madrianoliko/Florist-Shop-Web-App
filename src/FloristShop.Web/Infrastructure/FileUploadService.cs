using Microsoft.AspNetCore.Components.Forms;

namespace FloristShop.Web.Infrastructure;

/// <summary>
/// Serwis uploadu plików do wwwroot/uploads/{subfolder}/.
/// Używany przez admin do dodawania zdjęć do katalogu bukietów i rekwizytów.
///
/// Przechowywanie lokalne (wwwroot) — wystarczające dla małej kwiaciarni.
/// Przy migracji na serwer: kopiujesz folder uploads/.
/// Przy skalowaniu na wiele instancji: zamieniasz implementację na S3/R2 —
/// interfejs pozostaje taki sam (SaveAsync / Delete).
///
/// Wymaga IWebHostEnvironment — wstrzykiwany z DI.
/// Rejestracja: builder.Services.AddScoped&lt;FileUploadService&gt;() w Program.cs.
/// </summary>
public class FileUploadService(IWebHostEnvironment env)
{
    private static readonly HashSet<string> AllowedExtensions =
        [".jpg", ".jpeg", ".png", ".webp"];

    /// <summary>Maksymalny rozmiar pojedynczego pliku: 8 MB.</summary>
    private const long MaxFileSizeBytes = 8 * 1024 * 1024;

    /// <summary>
    /// Zapisuje plik w wwwroot/uploads/{subfolder}/ i zwraca ścieżkę relatywną
    /// do użycia w src="" obrazka (np. "/uploads/bouquets/abc123.jpg").
    ///
    /// subfolder: "bouquets" lub "rental" — nie może zawierać separatorów ścieżki.
    /// </summary>
    public async Task<string> SaveAsync(IBrowserFile file, string subfolder)
    {
        var ext = Path.GetExtension(file.Name).ToLowerInvariant();

        if (!AllowedExtensions.Contains(ext))
            throw new InvalidOperationException(
                $"Niedozwolony format pliku: {ext}. Dozwolone: jpg, jpeg, png, webp.");

        // Stwórz folder jeśli nie istnieje — Directory.CreateDirectory jest idempotentne
        var uploadsDir = Path.Combine(env.WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadsDir, fileName);

        await using var readStream = file.OpenReadStream(maxAllowedSize: MaxFileSizeBytes);
        await using var writeStream = File.Create(fullPath);
        await readStream.CopyToAsync(writeStream);

        return $"/uploads/{subfolder}/{fileName}";
    }

    /// <summary>
    /// Zapisuje surowe bajty (np. wynik getCroppedCanvas().toDataURL przekonwertowany z base64)
    /// i zwraca ścieżkę relatywną do użycia w src="" obrazka.
    /// Używane przez kadrowanie zdjęć (crop modal w BouquetTemplateForm / RentalItemForm).
    /// </summary>
    public async Task<string> SaveBytesAsync(byte[] data, string subfolder, string ext, CancellationToken ct = default)
    {
        if (!AllowedExtensions.Contains(ext))
            ext = ".jpg";

        var uploadsDir = Path.Combine(env.WebRootPath, "uploads", subfolder);
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploadsDir, fileName);

        await File.WriteAllBytesAsync(fullPath, data, ct);
        return $"/uploads/{subfolder}/{fileName}";
    }

    /// <summary>
    /// Usuwa plik z dysku na podstawie ścieżki relatywnej (np. "/uploads/bouquets/abc.jpg").
    /// Cicha operacja — jeśli plik nie istnieje, nic się nie dzieje.
    /// </summary>
    public void Delete(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;

        var fullPath = Path.Combine(
            env.WebRootPath,
            relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
