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
public class AuthorController:ControllerBase
{
    private readonly IAuthorRepository _AuthorRepository;
    private string AuthorImagesPath =>
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "AuthorImage");

    public  AuthorController(IAuthorRepository AuthorRepository)
    {
        _AuthorRepository = AuthorRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Act = await _AuthorRepository.GetAllAsync();
        return Ok(Act);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var Act = await _AuthorRepository.GetAuthorWithBooks(id);
        if (Act == null)
            return NotFound();

        return Ok(Act);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody]CreatAuthorReq Auth,IFormFile Imag)
    {

        if ( Imag.Length > 0)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), AuthorImagesPath, fileName);

                
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Imag.CopyToAsync(stream); 
            }

            Auth.Image = fileName;
        }
        var author = new Author
        {
            Name = Auth.Name,
            Email = Auth.Email,
            Image = Auth.Image,
            BirthDate = Auth.BirthDate,
            Description = Auth.Description,
            
        };
        var Aoth =  await _AuthorRepository.CreatAsync(author);

        return CreatedAtAction(nameof(GetById), new { id = Aoth.Id });
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var Author =await _AuthorRepository.GetByIdAsync(id);
    
        if (Author == null)
        {
            return NotFound();
        }
    
        // مسح الصورة
        if (!string.IsNullOrEmpty(Author.Image))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), AuthorImagesPath, Author.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        await _AuthorRepository.DeleteAsync(id);
        return RedirectToAction(nameof(GetAll));
    }

    
    [HttpPut("{Id}")]
    [Authorize]
    public async  Task<IActionResult> Edit(int Id, UpdateAuthorReq act,IFormFile Imag)
    {
        
        var Author = await _AuthorRepository.GetByIdAsync(Id);
        if (Author == null)
        {
            return NotFound();
        }
        if ( Imag is not null && Imag.Length > 0)
        {
            var oldfilePath = Path.Combine(Directory.GetCurrentDirectory(), AuthorImagesPath, Author.Image);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
            
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Imag.FileName) ;
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), AuthorImagesPath, fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Imag.CopyTo(stream);
            }

            Author.Image = fileName;
        }
        
        Author.Name = act.Name;
        Author.BirthDate = act.BirthDate;
        Author.Description = act.Description;
        Author.Email = act.Email;
        
        await _AuthorRepository.UpdateAsync(Author);
        
        return RedirectToAction(nameof(GetById), new { id = Author.Id });
    }
    
}