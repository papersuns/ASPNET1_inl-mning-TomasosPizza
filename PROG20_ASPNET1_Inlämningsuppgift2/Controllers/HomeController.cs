using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROG20_ASPNET1_Inlämningsuppgift2.Models;
using PROG20_ASPNET1_Inlämningsuppgift2.ModelsIdentity;
using PROG20_ASPNET1_Inlämningsuppgift2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG20_ASPNET1_Inlämningsuppgift2.Controllers
{
    public class HomeController : Controller
    {
        private TomasosContext _context;
        private UserManager<ApplicationUser> _usermanager;
        public HomeController(TomasosContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            var model = new MatrattProduktViewModel();
            model.Matratter = _context.Matratts.ToList();
            model.Ingredienser = _context.Produkts.ToList();
            model.MatrattProdukt = _context.MatrattProdukts.ToList();

            return View(model);
        }

        public IActionResult AddProductToCart(int id)
        {
            List<Matratt> cart;

            if (HttpContext.Session.GetString("ProductCart") == null)
            {
                cart = new List<Matratt>();
            }
            else
            {
                string jsonCart = HttpContext.Session.GetString("ProductCart");

                cart = JsonConvert.DeserializeObject<List<Matratt>>(jsonCart);
            }

            Matratt newProduct = _context.Matratts.SingleOrDefault(p => p.MatrattId == id);
            cart.Add(newProduct);

            string cartToJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("ProductCart", cartToJson);

            return RedirectToAction("Menu");
        }

        public IActionResult Checkout()
        {
             string cartJSON = HttpContext.Session.GetString("ProductCart");

            if (cartJSON == null)
            {
                return RedirectToAction("Menu");
            }
            else
            {
                List<Matratt> model2 = JsonConvert.DeserializeObject<List<Matratt>>(cartJSON);

                BestallningMatrattViewModel model = new BestallningMatrattViewModel();
                model.Matratter = model2;
                
                return View(model);
            }  
        }

        public async Task<IActionResult> CheckoutConfirm()
        {
            string cartJSON = HttpContext.Session.GetString("ProductCart");

            List<Matratt> Matratter = JsonConvert.DeserializeObject<List<Matratt>>(cartJSON);

            ApplicationUser user = await _usermanager.FindByNameAsync(User.Identity.Name);
            Kund currentKund = _context.Kunds.SingleOrDefault(x => x.Id == user.Id);

            Bestallning bestallning = new Bestallning();
            bestallning.BestallningDatum = DateTime.Now;
            bestallning.Totalbelopp = Matratter.Sum(p => p.Pris);
            bestallning.Levererad = false;
            bestallning.KundId = currentKund.KundId;

            _context.Bestallnings.Add(bestallning);
            _context.SaveChanges();

            Bestallning currentBestallning = _context.Bestallnings.SingleOrDefault(x => x.BestallningDatum == bestallning.BestallningDatum);
            List<int> existingMatrattId = new List<int>();

            foreach (var matratt in Matratter)
            {
                //check for duplicates, either update amount or create new
                if (existingMatrattId.Contains(matratt.MatrattId))
                {
                    BestallningMatratt existingBestallningmatratt = _context.BestallningMatratts.SingleOrDefault(x => x.BestallningId == currentBestallning.BestallningId && x.MatrattId == matratt.MatrattId);
                    existingBestallningmatratt.Antal++;
                    _context.BestallningMatratts.Update(existingBestallningmatratt);
                }
                else
                {
                    existingMatrattId.Add(matratt.MatrattId);
                    BestallningMatratt bestallningmatratt = new BestallningMatratt();
                    bestallningmatratt.BestallningId = currentBestallning.BestallningId;
                    bestallningmatratt.MatrattId = matratt.MatrattId;
                    bestallningmatratt.Antal = 1;

                    _context.BestallningMatratts.Add(bestallningmatratt);
                }

                _context.SaveChanges();
            }

            //empty cart
            HttpContext.Session.Remove("ProductCart");
            
            return View("Accepted");
        }
    }
}
