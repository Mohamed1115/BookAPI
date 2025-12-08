using BookAPI.Data;
using BookAPI.IRepositories;
using BookAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookAPI.IRepositories;
using BookAPI.Models;
using BookAPI.ModelView;
using Microsoft.AspNetCore.Identity;

namespace BookAPI.Controllers;
[Route("Auther/[controller]/[action]")]
[ApiController]
public class CommentController:ControllerBase
{
    private readonly ICommentRepository _CommentRepository;
    private readonly IBookRepository _BookRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    // private readonly IBookRepository _BookRepository;
    
    
    public  CommentController(ICommentRepository CommentRepository, IBookRepository bookRepository)
    {
        _CommentRepository = CommentRepository;
        _BookRepository = bookRepository;
    }

    // [HttpGet]
    // public async Task<IActionResult> GetAll()
    // {
    //     var Com = await _CommentRepository.GetAllAsync();
    //     return Ok(Com);
    // }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var Com = await _CommentRepository.GetCommentDetails(id);
        if (Com == null)
            return NotFound();

        return Ok(Com);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody]CreatCommentReq Com,int id)
    {
        var user = await _userManager.GetUserAsync(User);
        
        var Comment = new Comment
        {
            Text = Com.Text,
            UserId = user.Id,
            Author = user.Name,
            Email = user.Email,
            Message = Com.Message,
            Date = DateTime.UtcNow,
            BookId = id,
            Rating = Com.Rating,
            
        };
        var comm =  await _CommentRepository.CreatAsync(Comment);
        // var com = await _CommentRepository.GetCommentDetails(id);
        var bookRat = await _CommentRepository.GetAverageRatingAsync(id);
        var book = await _BookRepository.GetByIdAsync(id);
        book.ReviewsCount++;
        book.Rating = bookRat;
        
        await _BookRepository.UpdateAsync(book);

        return CreatedAtAction(nameof(GetById), new { id = comm.Id });
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var Comment =await _CommentRepository.GetByIdAsync(id);
    
        if (Comment == null)
        {
            return NotFound();
        }
    
        // مسح الصورة
        
        await _CommentRepository.DeleteAsync(id);
        var bookRat = await _CommentRepository.GetAverageRatingAsync(Comment.BookId);
        var book = await _BookRepository.GetByIdAsync(Comment.BookId);
        book.ReviewsCount--;
        book.Rating = bookRat;
        
        await _BookRepository.UpdateAsync(book);
        return RedirectToAction(nameof(GetById), new { id = Comment.Id });
    }

    
    [HttpPut("{Id}")]
    [Authorize]
    public async  Task<IActionResult> Edit(int Id, UpdateCommentReq Com)
    {
        
        var Comment = await _CommentRepository.GetByIdAsync(Id);
        if (Comment == null)
        {
            return NotFound();
        }
        

        Comment.Text = Com.Text;
        Comment.Message = Com.Message;
        Comment.Rating = Com.Rating;
        
        await _CommentRepository.UpdateAsync(Comment);
        var bookRat = await _CommentRepository.GetAverageRatingAsync(Comment.BookId);
        var book = await _BookRepository.GetByIdAsync(Comment.BookId);
        book.Rating = bookRat;
        await _BookRepository.UpdateAsync(book);
        
        return RedirectToAction(nameof(GetById), new { id = Comment.Id });
    }
    
}