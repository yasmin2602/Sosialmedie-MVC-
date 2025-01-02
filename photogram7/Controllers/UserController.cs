using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using photogram.Models;
using photogram.DAL;
using Microsoft.EntityFrameworkCore;

namespace photogram.Controllers
{
    [Authorize] // secures that every actions done in usercontroller needs authentication unless explicitly overridden
    public class UserController : Controller
    {
        //Postdbcontext uses to engages with the database for getting posts 
        //logger for informasjon, warnings, and errors
        private readonly PostDbContext _postDbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(PostDbContext postDbContext, ILogger<UserController> logger)
        {
            _postDbContext = postDbContext;
            _logger = logger;
        }

        //gets all posts  from database and let them to the feed view
        [HttpGet]
        public async Task<IActionResult> Feed()
        {
            var posts = await _postDbContext.Posts.ToListAsync();
            return View(posts);
        }

        //gets from database and let them in on postviewmodel. 
        public async Task<IActionResult> Posts(PostViewModel model)
        {
            _logger.LogInformation("Getting all posts");
            var posts = await _postDbContext.Posts.ToListAsync();
            return View(posts);
        }
    }
}
