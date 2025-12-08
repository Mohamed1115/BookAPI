using BookAPI.IRepositories;
using BookAPI.Models;
using BookAPI.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationAPI.Controllers;
[Route("Category/[controller]/[action]")]
[ApiController]
public class CategoryController:ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public  CategoryController(ICategoryRepository CategoryRepository)
    {
        _categoryRepository = CategoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cat = await _categoryRepository.GetAllAsync();
        return Ok(cat);
    }

    
    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var cat = await _categoryRepository.GetCategoryWithMovies(id);
        if (cat == null)
            return NotFound();

        return Ok(cat);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody]CreatCategory categoryReq)
    {
        var category = new Category();
        category.Name = categoryReq.Name;
        category.Description = categoryReq.Description;
        var add = await _categoryRepository.CreatAsync(category);
        
        if (add == null)
            return BadRequest();
        
        
        return Created();
        
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var category =await _categoryRepository.GetByIdAsync(id);
    
        if (category == null)
        {
            return NotFound();
        }
        
        await _categoryRepository.DeleteAsync(id);
        
        return Ok($"Category {category.Name} deleted successfully");
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Edit(int id,[FromBody]CreatCategory categoryReq)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        
        category.Name = categoryReq.Name;
        category.Description = categoryReq.Description;
        await _categoryRepository.UpdateAsync(category);
        return Ok();
    }
    
    
    
    
    
}