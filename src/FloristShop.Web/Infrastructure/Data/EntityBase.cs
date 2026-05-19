namespace FloristShop.Web.Infrastructure.Data;

/// <summary>
/// Wspólna klasa bazowa dla wszystkich encji w systemie.
/// Daje każdej encji: Id (Guid) + CreatedAt + UpdatedAt (oba w UTC).
///
/// Używamy abstract class zamiast interfejsu, bo:
/// - Implementacje pól (auto-properties) są wspólne dla WSZYSTKICH encji — nie ma sensu powtarzać.
/// - Łatwo dorzucić w przyszłości wspólną logikę (np. metodę Touch() do uaktualnienia UpdatedAt
///   z poziomu kodu domeny, gdyby zaszła potrzeba).
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Klucz główny. Generowany po stronie aplikacji (Guid.NewGuid()),
    /// dzięki czemu mamy ID jeszcze przed zapisem do bazy — przydatne dla referencji w pamięci.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Kiedy rekord został utworzony. UTC. Ustawiany raz, przy konstrukcji obiektu.
    /// (EF Core przy odczycie z bazy nadpisze to wartością z DB przez setter.)
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Kiedy rekord był ostatnio modyfikowany. UTC. Null przy pierwszym zapisie.
    /// Automatycznie ustawiany w AppDbContext.SaveChanges() na podstawie ChangeTracker.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
