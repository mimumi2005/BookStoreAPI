using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Controller for managing books.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly BookStoreDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="BooksController"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public BooksController(BookStoreDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all books from the database, or books by a specific author if the author query parameter is provided.
    /// </summary>
    /// <param name="author">The name of the author whose books to retrieve. If not provided, all books are retrieved.</param>
    /// <param name="page">The page number for pagination (defaults to 1 if not provided).</param>
    /// <returns>A paginated list of books. If an author is specified, returns books by that author, otherwise returns all books.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Books>>> GetBooks([FromQuery] string? author, [FromQuery] int page = 1)
    {
        int pageSize = 5; // Page size for pagination

        IQueryable<Books> query = _context.Books;

        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(b => b.Author == author); // Filter by author if provided
        }

        // Get the total count of books
        var totalBooks = await query.CountAsync();

        // Calculate total pages based on the page size
        var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);

        // If the page number is greater than the total pages, return a bad request
        if (page > totalPages && totalPages > 0)
        {
            return BadRequest("Page number exceeds total number of pages.");
        }

        // Get the paginated list of books
        var books = await query
            .Skip((page - 1) * pageSize) // Skip the books from the previous pages
            .Take(pageSize) // Take the number of books specified by pageSize
            .ToListAsync();

        return Ok(new
        {
            TotalBooks = totalBooks,
            TotalPages = totalPages,
            CurrentPage = page,
            Books = books
        });
    }


    /// <summary>
    /// Retrieves a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to retrieve.</param>
    /// <returns>A book object if found, or a 404 Not Found if not.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Books>> GetBook(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
            return NotFound();

        return book;
    }

    /// <summary>
    /// Adds a new book to the database.
    /// </summary>
    /// <param name="book">The book object to add, as JSON.</param>
    /// <returns>The newly created book object, including the generated ID.</returns>
    [HttpPost]
    public async Task<ActionResult<Books>> PostBook(Books book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

    /// <summary>
    /// Updates an existing book in the database.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="book">The updated book object to add, as JSON.</param>
    /// <returns>Returns No Content (204) if successful, or a Bad Request (400) if the ID mismatch occurs.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, Books book)
    {
        if (id != book.Id)
            return BadRequest();

        _context.Entry(book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Books.Any(e => e.Id == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a book by its ID.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <returns>No Content (204) if successful, or Not Found    (404) if the book is not found.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return NotFound();

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

