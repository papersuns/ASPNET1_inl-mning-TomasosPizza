using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG20_ASPNET1_Inlämningsuppgift2.Models;
using PROG20_ASPNET1_Inlämningsuppgift2.ModelsIdentity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG20_ASPNET1_Inlämningsuppgift2.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signinmanager;
        private UserManager<ApplicationUser> _usermanager;
        private IPasswordHasher<ApplicationUser> _passwordhasher;
        private TomasosContext _context;

        public AccountController(SignInManager<ApplicationUser> signinmanager, UserManager<ApplicationUser> usermanager, IPasswordHasher<ApplicationUser> passwordhasher, TomasosContext context)
        {
            _signinmanager = signinmanager;
            _usermanager = usermanager;
            _passwordhasher = passwordhasher;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Kund user)
        {
            ApplicationUser loginUser = new ApplicationUser() { UserName = user.AnvandarNamn };

            var result = await _signinmanager.PasswordSignInAsync(user.AnvandarNamn, user.Losenord, false, true);

            if (result.Succeeded)
            {
                return RedirectToAction("LoggedIn", "Account", new { model = user });
            }
            else
            {
                ViewBag.Message = "Felaktig inloggning";
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _signinmanager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Kund newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser newAccount = new ApplicationUser() 
                { 
                    UserName = newUser.AnvandarNamn, 
                    Email = newUser.Email,
                    PhoneNumber = newUser.Telefon,
                    Name = newUser.Namn,
                    StreetAddress = newUser.Gatuadress,
                    PostalCode = newUser.Postnr,
                    City = newUser.Postort
                };

                var result = await _usermanager.CreateAsync(newAccount, newUser.Losenord);

                if (result.Succeeded)
                {
                    await _signinmanager.SignInAsync(newAccount, isPersistent: false);

                    newUser.Id = newAccount.Id;
                    _context.Kunds.Add(newUser);
                    _context.SaveChanges();

                    return RedirectToAction("LoggedIn", "Account", new { model = newUser});
                }
            }

            ViewBag.Message = "Något gick fel. Försök igen";
            return View();       
        }

        [Authorize]
        public async Task<IActionResult> LoggedIn()
        {
            ApplicationUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(user);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LoggedIn(string email, string phonenumber, string name, string streetaddress, string postalcode, string city, string passwordhash)
        {
            if (passwordhash == null)
            {
                ViewBag.Message = "Fyll i lösenord";
                return RedirectToAction("LoggedIn");
            }
            ApplicationUser user = await _usermanager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                user.Email = email;
                user.PhoneNumber = phonenumber;
                user.Name = name;
                user.StreetAddress = streetaddress;
                user.PostalCode = postalcode;
                user.City = city;
                user.PasswordHash = _passwordhasher.HashPassword(user, passwordhash);

                IdentityResult result = await _usermanager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    Kund currentKund = _context.Kunds.SingleOrDefault(x => x.Id == user.Id);
                    currentKund.Email = email;
                    currentKund.Telefon = phonenumber;
                    currentKund.Namn = name;
                    currentKund.Gatuadress = streetaddress;
                    currentKund.Postnr = postalcode;
                    currentKund.Postort = city;
                    currentKund.Losenord = passwordhash;

                    _context.Update(currentKund);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Message = "Något gick fel";
            return RedirectToAction("LoggedIn");
        }
    }
}
