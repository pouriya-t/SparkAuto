using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting.Internal;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private IHostingEnvironment _environment;

        [BindProperty]
        public Car Car { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        
        public CreateModel(ApplicationDbContext db,IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public IActionResult OnGet(string userId = null)
        {
            Car = new Car();

            if(userId == null)
            {
                var claimsIdenity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdenity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }

            Car.UserId = userId;

            return Page();
        }

        
        public async Task<IActionResult> OnPostAsync()
        {
            

            if (!ModelState.IsValid)
            {
                return Page();
            }


            string fileName=Guid.NewGuid().ToString() + "_" + Photo.FileName;
            var file = Path.Combine(_environment.WebRootPath, "uploads", fileName);

            Photo.CopyTo(new FileStream(file, FileMode.Create));


            Car.Image = fileName;

            _db.Car.Add(Car);
            await _db.SaveChangesAsync();
            StatusMessage = "Car has been added sucessfully";

            return RedirectToPage("Index", new { userId = Car.UserId });
        }

    }
}