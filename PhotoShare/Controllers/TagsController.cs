using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public TagsController(PhotoShareContext context)
        {
            _context = context;
        }

        // Removed the following action methods:
        // GET: Tags
        // GET: Tags/Details/5 
        // GET: Tags/Edit/5
        // POST: Tags/Edit/5
        // POST: Tags/Delete/5


        // GET: Tags/Create/5
        // id = PhotoId
        public IActionResult Create(int? id)
        {
            if (id == null)
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
