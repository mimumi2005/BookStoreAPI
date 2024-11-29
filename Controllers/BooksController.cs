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
    /// Retrieves all books from the database.
    /// </summary>
    /// <returns>A list of books.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Books>>> GetBooks()
    {
        return await _context.Books.ToListAsync();
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

