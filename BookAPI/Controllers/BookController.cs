using BookAPI.IRepositories;
using BookAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookAPI.IRepositories;
using BookAPI.Models;
using BookAPI.ModelView;

namespace BookAPI.Controllers;
[Route("Auther/[controller]/[action]")]
[ApiController]
public class BookController:ControllerBase
{
    private readonly IBookRepository _BookRepository;
    private string BookImagesPath =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "BookImage");

    public  BookController(IBookRepository BookRepository)
    {
        _BookRepository = BookRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? category ,string? BookName,string? Author,int? Year ,bool rating=false,int page = 1)
    {
        var bok = await _BookRepository.GetAllBook();
        if (BookName is not null)
            bok = bok.Where(e => e.Title.Contains(BookName)).ToList();
        if (Author is not null)
            bok = bok.Where(e => e.Author.Name.Contains(Author)).ToList();
        if (Year is not null)
            bok = bok.Where(e => e.YearPublished==Year).ToList();
        if (category is not null)
            bok = bok.Where(e => e.Category.Name==category).ToList();
        if (rating)
            bok=bok.OrderByDescending(e => e.Rating).ToList();
        int totalPages = (int)Math.Ceiling((double)bok.Count() / 10);
        bok = bok.Skip((page - 1) * 10).Take(10).ToList(); 
        return Ok(new
        {
            Books = bok,
            TotalPages = totalPages
        } );
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var Act = await _BookRepository.GetBookDetails(id);
        if (Act == null)
            return NotFound();

        return Ok(Act);
    }

    [HttpPost]
    [Authorize(Roles = $"{SD.SuperAdminRole},{SD.AdminRole}")]
    public async Task<IActionResult> Create([FromForm]CreatBookReq bok,IFormFile Imag)
    {

        if ( Imag.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), BookImagesPath, fileName);

                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Imag.CopyToAsync(stream); 
            }

            bok.ImageUrl = fileName;
        }
        var book = new Book
        {
            Title = bok.Title,
            ImageUrl = bok.ImageUrl,
            Price = bok.Price,
            Description = bok.Description,
            AuthorId = bok.AuthorId,
            CategoryId =  bok.CategoryId,
            Quantity=bok.Quantity
            
        };
        var Aoth =  await _BookRepository.CreatAsync(book);

        return CreatedAtAction(nameof(GetById), new { id = Aoth.Id });
    }

    [HttpDelete]
    [Authorize(Roles = $"{SD.SuperAdminRole},{SD.AdminRole}")]
    public async Task<IActionResult> Delete(int id)
    {
        var Book =await _BookRepository.GetByIdAsync(id);
    
        if (Book == null)
        {
            return NotFound();
        }
    
        // مسح الصورة
        if (!string.IsNullOrEmpty(Book.ImageUrl))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), BookImagesPath, Book.ImageUrl);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        await _BookRepository.DeleteAsync(id);
        return RedirectToAction(nameof(GetAll));
    }

    
    [HttpPut("{Id}")]
    [Authorize(Roles = $"{SD.SuperAdminRole},{SD.AdminRole}")]
    public async  Task<IActionResult> Edit(int Id, UpdateBookReq bok,IFormFile Imag)
    {
        
        var Book = await _BookRepository.GetByIdAsync(Id);
        if (Book == null)
        {
            return NotFound();
        }
        if ( Imag is not null && Imag.Length > 0)
        {
            var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), BookImagesPath, Book.ImageUrl);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
            
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), BookImagesPath, fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Imag.CopyTo(stream);
            }

            Book.ImageUrl = fileName;
        }

        Book.Title = bok.Title;
        Book.ImageUrl = bok.ImageUrl;
        Book.Price = bok.Price;
        Book.Description = bok.Description;
        Book.AuthorId = bok.AuthorId;
        Book.CategoryId = bok.CategoryId;
        Book.Quantity = bok.Quantity;
        
        await _BookRepository.UpdateAsync(Book);
        
        return RedirectToAction(nameof(GetById), new { id = Book.Id });
    }
    
}