using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Data;
using PasswordManager.Models;
using PasswordManager.Utility;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace PasswordManager.Controllers
{
    public class PasswordEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public PasswordEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 15; 
            int pageNumber = page ?? 1; 

            var passwordEntries = await _context.PasswordEntries.ToListAsync();

            var pagedPasswordEntries = passwordEntries.ToPagedList(pageNumber, pageSize);

            return View(pagedPasswordEntries);
        }


        public IActionResult Create()
        {
            return View();
        }


        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                return Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Site,PasswordFor,EmailAddress")] PasswordEntry passwordEntry)
        {
            passwordEntry.Site = passwordEntry.Site.ToUpper();
            passwordEntry.PasswordFor = passwordEntry.PasswordFor.ToUpper();

            var existingEntry = await _context.PasswordEntries
                .FirstOrDefaultAsync(pe => pe.Site == passwordEntry.Site && pe.PasswordFor == passwordEntry.PasswordFor);

            if (existingEntry != null)
            {
                TempData["Exists"] = "Password for this site for this owner already exists.";
                return View(passwordEntry);
            }



            if (ModelState.IsValid)
            {
                passwordEntry.Password = PasswordGenerator.GeneratePassword();
                var hashedPassword = HashPassword(passwordEntry.Password);

                bool isVerified = VerifyPassword(passwordEntry.Password, hashedPassword);

                passwordEntry.Password = hashedPassword;

                _context.Add(passwordEntry);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Password generated and saved successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["Failed"] = "Could not generate password. An error occured !";

            return View(passwordEntry);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var passwordEntry = await _context.PasswordEntries.FindAsync(id);
            if (passwordEntry == null) return NotFound();


            ViewBag.MaskedPassword = new string('*', passwordEntry.Password.Length);
            ViewBag.MaskedEmail = new string('*', passwordEntry.EmailAddress.Length - 3) + passwordEntry.EmailAddress.Substring(passwordEntry.EmailAddress.Length - 3);

            return View(passwordEntry);
        }


        [HttpPost]
        public async Task<IActionResult> RevealPassword(int id, string emailInput)
        {
            var passwordEntry = await _context.PasswordEntries.FindAsync(id);
            if (passwordEntry != null && passwordEntry.EmailAddress == emailInput)
            {
                return Json(new { success = true, password = passwordEntry.Password });
            }

            return Json(new { success = false });
        }
    }
}
