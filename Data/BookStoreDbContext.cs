using Microsoft.EntityFrameworkCore;

/// <summary>
/// Database context for the BookStore application.
/// </summary>
public class BookStoreDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the Books DbSet.
    /// </summary>
    required public DbSet<Books> Books { get; set; }

    /// <summary>
    /// Gets the path to the database file.
    /// </summary>
    public string DbPath { get; }


    /// <summary>
    /// Initializes a new instance of the <see cref="BookStoreDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }


    /// <summary>
    /// Gets or sets the Books entity.
    /// </summary>
    public DbSet<Books> Book { get; set; }

    /// <summary>
    /// Configures the database context options.
    /// </summary>
    /// <param name="optionsBuilder">The options builder.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}