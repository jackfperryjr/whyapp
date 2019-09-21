using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WhyApp.Models;
using WhyApp.Data;

namespace WhyApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public IActionResult Admin(string searchString)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            var users = (from user in _context.Users  
                        select new  
                        {  
                            UserId = user.Id,    
                            UserName = user.UserName,                                    
                            FirstName = user.FirstName,  
                            LastName = user.LastName,
                            Picture = user.Picture,
                            Email = user.Email,
                            City = user.City,
                            State = user.State,
                            ZipCode = user.ZipCode
                        }).ToList()
                        .Select(u => new ApplicationUser()  
                        {  
                            Id = u.UserId,  
                            UserName = u.UserName,
                            FirstName = u.FirstName, 
                            LastName = u.LastName, 
                            Picture = u.Picture,
                            Email = u.Email,
                            City = u.City,
                            State = u.State,
                            ZipCode = u.ZipCode
                        }).Where(u => u.Id != userId);  
                        
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                users = users.Where(u => {
                                            try 
                                            {
                                                return u.UserName.Contains(searchString)
                                                || u.City.ToLower().Contains(searchString)
                                                || u.ZipCode.Contains(searchString);
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        }
                                    ); 
            }

            return View(users.ToList());  
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> EditUserRole(string id, int role)  
        { 
            var userRole = "";
            if (role == 1) 
            {
                userRole = "Admin";
            }
            else if (role == 2) 
            {
                userRole = "SuperUser";
            }
            else 
            {
                userRole = "User";
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            await _userManager.RemoveFromRoleAsync(user, "SuperUser");
            await _userManager.RemoveFromRoleAsync(user, "User");

            await _userManager.AddToRoleAsync(user, userRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }
        public IActionResult Friends(string searchString)
        {
            var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

            var users = (from user in _context.Users  
                        select new  
                        {  
                            UserId = user.Id,    
                            UserName = user.UserName,                                    
                            FirstName = user.FirstName,  
                            LastName = user.LastName,
                            Picture = user.Picture,
                            Email = user.Email,
                            City = user.City,
                            State = user.State,
                            ZipCode = user.ZipCode
                        }).ToList()
                        .Select(u => new ApplicationUser()  
                        {  
                            Id = u.UserId,  
                            UserName = u.UserName,
                            FirstName = u.FirstName, 
                            LastName = u.LastName, 
                            Picture = u.Picture,
                            Email = u.Email,
                            City = u.City,
                            State = u.State,
                            ZipCode = u.ZipCode
                        }).Where(u => u.Id != userId);  

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                users = users.Where(u => {
                                            try 
                                            {
                                                return u.UserName.Contains(searchString)
                                                || u.City.ToLower().Contains(searchString)
                                                || u.ZipCode.Contains(searchString);
                                            }
                                            catch
                                            {
                                                return false;
                                            }
                                        }
                                    ); 
            }

            return View(users.ToList()); 
        }
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
 
            return View(user);        
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
