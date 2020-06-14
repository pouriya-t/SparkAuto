using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Cars
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;


        [BindProperty]
        public Car Car { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Car = await _db.Car.Include(c => c.Application)
                .FirstOrDefaultAsync(m => m.Id == id);

            if(Car == null)
            {
                return NotFound();
            }

            return Page();

        }

        public async Task<IActionResult> OnPost()
        {

            if(Car == null)
            {
                return NotFound();
            }

            var userId = Car.UserId;

            _db.Car.Remove(Car);
            await _db.SaveChangesAsync();

            StatusMessage = "Car deleted successfully.";

            return RedirectToPage("./Index",new { userId});
        }
    }
}