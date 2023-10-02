using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;

namespace Blog.Controllers
{
    public class TagController : Controller
    {
        private ApplicationDbContext _context;
        public TagController(ApplicationDbContext applicationDbContext) 
        {
            this._context = applicationDbContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View(); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(Tag tag)
        {   
            _context.Tags.Add(tag);
            _context.SaveChanges();

            return View("Success");
        }
    }
}
