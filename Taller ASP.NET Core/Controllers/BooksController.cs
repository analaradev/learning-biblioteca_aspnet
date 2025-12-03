using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taller_ASP.NET_Core.Data;
using Taller_ASP.NET_Core.Models;

namespace Taller_ASP.NET_Core.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<BooksController> _logger;

        public BooksController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<BooksController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // ==================== INDEX (CON BÚSQUEDA Y FILTROS) ====================

        public async Task<IActionResult> Index(string searchTerm, string filter = "all")
        {
            var userId = _userManager.GetUserId(User);
            var query = _context.Books.Where(b => b.UserId == userId);

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchTerm) ||
                    (b.Author != null && b.Author.Contains(searchTerm)) ||
                    (b.ISBN != null && b.ISBN.Contains(searchTerm)) ||
                    (b.Genre != null && b.Genre.Contains(searchTerm)) ||
                    (b.Description != null && b.Description.Contains(searchTerm))
                );
            }

            // Aplicar filtro de estado
            switch (filter?.ToLower())
            {
                case "pending":
                    query = query.Where(b => !b.IsRead);
                    break;
                case "completed":
                    query = query.Where(b => b.IsRead);
                    break;
            }

            var books = await query.OrderBy(b => b.Order).ToListAsync();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.Filter = filter ?? "all";

            return View(books);
        }

        // ==================== GET DETAIL ====================

        public async Task<IActionResult> GetBookDetail(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (book.UserId != userId)
            {
                _logger.LogWarning($"Acceso no autorizado a libro {id} por usuario {userId}");
                return Forbid();
            }

            return PartialView("_BookDetail", book);
        }

        // ==================== CREATE ====================

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            try
            {
                book.UserId = _userManager.GetUserId(User) ?? string.Empty;
                book.CreatedAt = DateTime.Now;

                // Procesar portada si existe
                if (book.ImageFile != null && book.ImageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await book.ImageFile.CopyToAsync(memoryStream);
                    book.CoverImage = memoryStream.ToArray();
                    book.ImageContentType = book.ImageFile.ContentType;
                }

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear libro");
                ModelState.AddModelError("", "Error al guardar el libro. Por favor, intenta de nuevo.");
                return View(book);
            }
        }

        // ==================== EDIT ====================

        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (book.UserId != userId)
            {
                _logger.LogWarning($"Intento de edición no autorizada de libro {id} por usuario {userId}");
                return Forbid();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (existingBook.UserId != userId)
            {
                _logger.LogWarning($"Intento de edición no autorizada de libro {id} por usuario {userId}");
                return Forbid();
            }

            try
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.Publisher = book.Publisher;
                existingBook.Year = book.Year;
                existingBook.Genre = book.Genre;
                existingBook.Description = book.Description;

                // Procesar portada si existe
                if (book.ImageFile != null && book.ImageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await book.ImageFile.CopyToAsync(memoryStream);
                    existingBook.CoverImage = memoryStream.ToArray();
                    existingBook.ImageContentType = book.ImageFile.ContentType;
                }

                _context.Update(existingBook);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al editar libro {id}");
                ModelState.AddModelError("", "Error al guardar los cambios. Por favor, intenta de nuevo.");
                return View(book);
            }
        }

        // ==================== DELETE ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (book.UserId != userId)
            {
                _logger.LogWarning($"Intento de eliminación no autorizada de libro {id} por usuario {userId}");
                return Forbid();
            }

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar libro {id}");
                TempData["Error"] = "Error al eliminar el libro. Por favor, intenta de nuevo.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ==================== TOGGLE READ ====================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleRead(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (book.UserId != userId)
            {
                _logger.LogWarning($"Intento no autorizado de cambiar estado de libro {id} por usuario {userId}");
                return Forbid();
            }

            try
            {
                book.IsRead = !book.IsRead;
                _context.Update(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cambiar estado de libro {id}");
                TempData["Error"] = "Error al cambiar el estado del libro. Por favor, intenta de nuevo.";
                return RedirectToAction(nameof(Index));
            }
        }

        // ==================== UPDATE ORDER ====================

        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromBody] List<int> bookIds)
        {
            if (bookIds == null || bookIds.Count == 0)
            {
                return BadRequest(new { success = false, message = "No se recibieron IDs" });
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                int updated = 0;

                for (int i = 0; i < bookIds.Count; i++)
                {
                    var book = await _context.Books.FindAsync(bookIds[i]);
                    if (book != null && book.UserId == userId)
                    {
                        book.Order = i;
                        updated++;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { success = true, updated = updated });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar orden de libros");
                return StatusCode(500, new { success = false, message = "Error al actualizar el orden" });
            }
        }
    }
}