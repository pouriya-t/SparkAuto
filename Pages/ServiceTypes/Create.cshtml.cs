using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SparkAuto.Data;
using SparkAuto.Models;
using SparkAuto.Utility;

namespace SparkAuto
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ServiceType ServiceType { get; set; }


        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ServiceType serviceType)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.ServiceType.Add(serviceType);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}