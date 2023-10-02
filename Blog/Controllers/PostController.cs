using Blog.Data;
using Blog.Models;
using Blog.Models.ModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PostController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page=1)
        {
            const int pageSize = 1;

            var posts = await _context.Posts
             .Include(p => p.Tags)
             .ToListAsync();

            int count = posts.Count;
            var pager = new Pager(count, page, pageSize);


            if (page < 1)
            {
                page = 1;
                pager.CurrentPage = page;
            }
            else if (page > pager.TotalPages)
            {
                page = pager.TotalPages;
                pager.CurrentPage = page;
            }
            this.ViewBag.Pager = pager;

            int StartCount = (int)(page - 1 * pageSize);
            List<Post> postsList = posts.GetRange(StartCount, pageSize);

            return _context.Posts != null ? View(postsList) : Problem("Could not find dbcontext");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var p = new PostTagsModelView();
            List<Tag> tags = await _context.Tags.ToListAsync();
            p.Tags = tags;

            return View(p);
        }
        [Authorize]
        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(Post p, String[] taggedItems)
        {

            Post post = p;
            foreach (String taggedID in taggedItems)
            {
                Guid tagID = Guid.Parse(taggedID);

                IQueryable<Tag> tags = _context.Tags.Where(id => id.ID == tagID);
                foreach (Tag tag in tags)
                {
                    post.Tags.Add(tag);
                }
            }

            post.Author = HttpContext.User.Identity.Name;
            post.PublishedDate = DateTime.Now;
            //post.UrlHandle = "/diwjao";
            post.Visible = true;

            _context.Posts.Add(post);
            _context.SaveChanges();

            return View("Success");
        }

        [HttpPost]
        [ActionName("Comment")]
        public IActionResult Comment(CommentPostModelView cpvm, String content)
        {
            // Guid guid = Guid.Parse(id);
            //Post post = _context.Posts.Include(t => t.Tags).Include(b => b.Comments).FirstOrDefault(id => id.ID == guid);

            Comment c = new Comment();
            c.Author = HttpContext.User.Identity.Name;
            c.Content = content;
            c.Posted = DateTime.Now;
            c.Post = _context.Posts.Single(x => x.ID == cpvm.Post.ID);

            _context.Comments.Add(c);
            _context.SaveChanges();

            return View("Success");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int page, String? id)
        {

            var post = await _context.Posts
            .Include(a => a.Tags)
            .Include(c => c.Comments)
            .FirstOrDefaultAsync(m => m.ID == Guid.Parse(id));

            if (post == null)
            {
                return NotFound();
            }
            const int pageSize = 1;

            int count = post.Comments.Count();
            var pager = new Pager(count, page, pageSize);


            if (page < 1)
            {
                page = 1;
                pager.CurrentPage = page;
            }
            else if (page > pager.TotalPages)
            {
                page = pager.TotalPages;
                pager.CurrentPage = page;
            }
            this.ViewBag.Pager = pager;

            int StartCount = (int)(page - 1 * pageSize);

            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }


            CommentPostModelView commentPostModelView = new CommentPostModelView();
            commentPostModelView.Post = post;
            commentPostModelView.Comments = post.Comments.ToList().GetRange(StartCount, pageSize);

            return View(commentPostModelView);
        }

        [Authorize]
        public async Task<IActionResult> Edit(String? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }
            Guid guid = Guid.Parse(id);

            //var post = await _context.Posts.FindAsync(guid);
            var post = await _context.Posts
            .Include(a => a.Tags)
            .FirstOrDefaultAsync(m => m.ID == Guid.Parse(id));

            if (post == null)
            {
                return NotFound();
            }
            PostTagsModelView postTagsModelView = new PostTagsModelView();
            postTagsModelView.Post = post;
            postTagsModelView.Tags =  await _context.Tags.ToListAsync();


            return View(postTagsModelView);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(String id, Post post, String[] taggedItems)
        {
            //Post post = postTagsModelView.Post;
            Guid guid = Guid.Parse(id);

            //Post postToBeUpdated = await _context.Posts.Include(p => p.Tags).FirstOrDefaultAsync(p => p.ID == guid);
            // postToBeUpdated.Tags.Clear();

            Post postToBeUpdated = _context.Posts.Include(t => t.Tags).FirstOrDefault(id => id.ID == guid);
            postToBeUpdated.Tags.Clear();
            //await _context.SaveChangesAsync();


            post.Tags = new List<Tag>();
            foreach (String taggedID in taggedItems)
            {
                Guid tagID = Guid.Parse(taggedID);

                Tag t = _context.Tags.Where(id => id.ID == tagID).FirstOrDefault();
                post.Tags.Add(t);
            }
            String name = HttpContext.User.Identity.Name;

            postToBeUpdated.Heading = post.Heading;
            postToBeUpdated.PageTitle = post.PageTitle;
            postToBeUpdated.Content = post.Content;
            postToBeUpdated.ShortDescription = post.ShortDescription;
            postToBeUpdated.Tags = post.Tags;
            postToBeUpdated.PublishedDate = DateTime.Now;
            //postToBeUpdated.Author = HttpContext.User;

            if (guid != post.ID)
            {
                return NotFound();
            }

            post.Author = "Sandesh";


            try
            {
                _context.Update(postToBeUpdated);
                await _context.SaveChangesAsync();
            }
             catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(guid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
           return View("Success");
        }

        [HttpGet]
        [ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> Delete(String? id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Room'  is null.");
            }
            Guid guid = Guid.Parse(id);
            var post = await _context.Posts.FindAsync(guid);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return View("Success");
        }

        private bool PostExists(Guid id)
        {
            return (_context.Posts?.Any(e => e.ID == id)).GetValueOrDefault();
        }

    }


}
