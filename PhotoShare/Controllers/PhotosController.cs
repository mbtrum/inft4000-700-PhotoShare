using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoShare.Data;
using PhotoShare.Models;

namespace PhotoShare.Controllers
{
    // Requires logged in user to access controller
    [Authorize]
    public class PhotosController : Controller
    {
        private readonly PhotoShareContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PhotosController(PhotoShareContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Removed the following action methods:
        // GET: Photos/Details/5


        // GET: Photos by user ID
        public async Task<IActionResult> Index()
        {
            // Get ID of person logged in
            var userId = _userManager.GetUserId(User);

            var photos = await _context.Photo
                .Where(m => m.ApplicationUserId == userId) // refine by user ID
                .ToListAsync();

            return View(photos);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoId,Description,Location,Camera,ImageFile,IsVisible")] Photo photo)
        {
            // initialize the datetime property
            photo.CreatedAt = DateTime.Now;

            // rename the uploaded file to a guid (unique filename). Set before photo saved in database.
            photo.ImageFilename = Guid.NewGuid().ToString() + Path.GetExtension(photo.ImageFile?.FileName);

            // Set the owner of the record for person logged in
            var userId = _userManager.GetUserId(User);
            photo.ApplicationUserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(photo);
                await _context.SaveChangesAsync();

                // Save the uploaded file after the photo is saved in the database.
                if (photo.ImageFile != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", photo.ImageFilename);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.ImageFile.CopyToAsync(fileStream);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get ID of person logged in
            var userId = _userManager.GetUserId(User);

            var photo = await _context.Photo
                .Include("Tags")
                .Where(m => m.ApplicationUserId == userId) // refine by user ID
                .FirstOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,Description,Location,Camera,ImageFilename,IsVisible,CreatedAt,ApplicationUserId")] Photo photo)
        {
            if (id != photo.PhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.PhotoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get ID of person logged in
            var userId = _userManager.GetUserId(User);
            
            var photo = await _context.Photo
                .Where(m => m.ApplicationUserId == userId) // refine by user ID
                .FirstOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get ID of person logged in
            var userId = _userManager.GetUserId(User);

            var photo = await _context.Photo
                .Where(m => m.ApplicationUserId == userId) // refine by user ID
                .FirstOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }
            else
            {
                _context.Photo.Remove(photo);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.PhotoId == id);
        }
    }
}
