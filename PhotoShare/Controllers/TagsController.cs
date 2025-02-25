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
    public class TagsController : Controller
    {
        private readonly PhotoShareContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TagsController(PhotoShareContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Removed the following action methods:
        // GET: Tags
        // GET: Tags/Details/5 
        // GET: Tags/Edit/5
        // POST: Tags/Edit/5
        // POST: Tags/Delete/5


        // GET: Tags/Create/5
        // id = PhotoId
        public async Task<IActionResult> Create(int? id)
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

            // Set the PhotoId for the Tag's fk
            ViewData["PhotoId"] = id;

            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TagId,Name,PhotoId")] Tag tag)
        {
            //
            // TO-DO: Get the photo and ensure logged in user is owner
            //

            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();

                // Re-direct back to the Photo's Edit page. /Photos/Edit/5
                return RedirectToAction("Edit", "Photos", new { id = tag.PhotoId });
            }

            // Set the PhotoId for the Tag's fk
            ViewData["PhotoId"] = tag.PhotoId;
            
            return View(tag);
        }       

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //
            // TO-DO: Get the photo and ensure logged in user is owner
            //

            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag
                .Include(t => t.Photo)               
                .FirstOrDefaultAsync(m => m.TagId == id);

            if (tag == null)
            {
                return NotFound();
            }

            // Remove tag from database
            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();

            // re-direct back to photo edit page
            return RedirectToAction("Edit", "Photos", new { id = tag.PhotoId });
        }
       
        private bool TagExists(int id)
        {
            return _context.Tag.Any(e => e.TagId == id);
        }
    }
}
