using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly PhotoShareContext _context;

        // Constructor
        public HomeController(PhotoShareContext context)
        {
            _context = context;
        }

        // Home page - display all photos
        public async Task<IActionResult> Index()
        {
            // get all photos
            var photos = await _context.Photo.ToListAsync();

            return View(photos); // pass photo list to view
        }

        // Photo Details - display the details of a photo
        public async Task<IActionResult> PhotoDetails(int id)
        {
            // get photo by id
            var photo = await _context.Photo
                .Include(m => m.Tags)
                .FirstOrDefaultAsync(m => m.PhotoId == id);

            return View(photo); // pass in photo to view
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
