using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Models.ViewModel;

namespace SparkAuto.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _environment;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public Car Car { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public EditModel(ApplicationDbContext db,IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            Car = await _db.Car
                .Include(c => c.Application).FirstOrDefaultAsync(m => m.Id == id);

            if (Car == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            if(Photo != null)
            {
                string oldName = Car.Image;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", oldName);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                string fileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                var file = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                Photo.CopyTo(new FileStream(file, FileMode.Create));

                Car.Image = fileName;
            }

            _db.Attach(Car).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            StatusMessage = "Car updated successfully.";

            return RedirectToPage("./Index", new { userId = Car.UserId });
        }
    }
}