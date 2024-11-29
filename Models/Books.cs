using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a book entity.
/// </summary>
public class Books
{
    /// <summary>
    /// Gets or sets the book ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the book.
    /// </summary>
    [Required]
    [MaxLength(100)] // Limits the title length to 100
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the author of the book.
    /// </summary>
    [Required]
    [MaxLength(100)] // Limits the author length to 100
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the price of the book.
    /// </summary>
    [Required]
    [Range(0.1, 10000)] // Not sure what the max price of a book would be, but for this example, 10000 will be the max
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the published date of the book.
    /// </summary>
    public DateTime PublishedDate { get; set; }
}