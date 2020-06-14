using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Utility;

namespace SparkAuto.Pages.Users
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id.Trim().Length == 0)
            {
                return NotFound();
            }

            ApplicationUser =await _db.ApplicationUser.FirstOrDefaultAsync(m => m.Id == id);

            if(ApplicationUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var userInDb = _db.ApplicationUser.SingleOrDefault(m => m.Id == ApplicationUser.Id);

            if (userInDb == null)
            {
                return NotFound();
            }

            _db.Users.Remove(userInDb);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}