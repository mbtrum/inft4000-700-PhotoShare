using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PhotoShare.Models;

namespace PhotoShare.Controllers
{
    public class HomeController : Controller
    {        

        // Constructor
        public HomeController()
        {            
        }

        // Home page - display all photos
        public IActionResult Index()
        {
            // Create a list of photos
            List<Photo> photos = new List<Photo>();

            // Create a photo
            Photo photo1 = new Photo();
            photo1.PhotoId = 1;
            photo1.Description = "Description 1";
            photo1.ImageFilename = "photo1.jpg";
            photo1.CreatedAt = DateTime.Now;

            Photo photo2 = new Photo();
            photo2.PhotoId = 2;
            photo2.Description = "Description 2";
            photo2.ImageFilename = "photo2.jpg";
            photo2.CreatedAt = DateTime.Now;

            Photo photo3 = new Photo();
            photo3.PhotoId = 3;
            photo3.Description = "Description 3";
            photo3.ImageFilename = "photo3.jpg";
            photo3.CreatedAt = DateTime.Now;

            // Add the photos to the list
            photos.Add(photo1);
            photos.Add(photo2);
            photos.Add(photo3);

            return View(photos); // pass photo list to view
        }

        // Photo Details - display the details of a photo
        public IActionResult PhotoDetails(int id)
        {
            // Create a photo
            Photo photo = new Photo();
            photo.PhotoId = id;
            photo.Description = "This is my cat.";
            photo.ImageFilename = "cat.jpg";
            photo.CreatedAt = DateTime.Now;

            return View(photo); // pass in photo to view
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
