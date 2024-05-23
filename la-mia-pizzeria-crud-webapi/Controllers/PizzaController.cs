using la_mia_pizzeria_crud_webapi.Context;
using la_mia_pizzeria_crud_webapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace la_mia_pizzeria_crud_webapi.Controllers
{
    public class PizzaController : Controller
    {
       
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PizzaController()
        {


        }


        public IActionResult Index()
        {
            return View(PizzaManager.GetAllPizza(true));
        }

        // Action per visualizzare i dettagli di una pizza

        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult Details(int id)
        {
            //restituiscimi la prima pizza con id uguale a quello passato
            var pizza = PizzaManager.GetPizza(id);

            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        [HttpGet]
        public IActionResult Popolate()
        {
            // Popolare la tabella ingredienti
            PizzaManager.SeedIng();
            // Popolare il database con dati di esempio se non ci sono pizze
            PizzaManager.Seed();

            return RedirectToAction("Index");

        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete()
        {
            PizzaManager.DeleteAllPizza();

            return RedirectToAction("Index");

        }


        // Action che fornisce la view con la form
        // per creare un nuovo post del blog
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {

            PizzaFormModel model = new PizzaFormModel();

            model.Pizza = new Pizza();

            model.Categories = PizzaManager.GetAllCategory();

            model.CreateIngredients();

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> Create(PizzaFormModel data, IFormFile foto)
        {

            if (!ModelState.IsValid)
            {

                // Ottenere la lista degli errori di validazione
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                // Verifica se ci sono errori o se la foto non è presente
                if (errorMessages.Count > 1 || foto == null || foto.Length == 0)
                {
                    data.Categories = PizzaManager.GetAllCategory();

                    data.CreateIngredients();

                    return View("Create", data);
                }

            }


            string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(foto.FileName);
            string imgPath = Path.Combine(imgFolderPath, imgFileName);

            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                await foto.CopyToAsync(stream);
            }
            //pizza.Pizza.Foto;

            data.Pizza.Foto = "~/img/" + imgFileName;

            PizzaManager.InsertPizza(data.Pizza, data.SelectedIngredients);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Update(int id)
        {
            // Prendo il post AGGIORNATO da database, non
            // uno passato da utente alla action
            var pizzaToEdit = PizzaManager.GetPizza(id);

            if (pizzaToEdit == null)
            {
                return NotFound();
            }

            else
            {
                PizzaFormModel model = new PizzaFormModel(pizzaToEdit, PizzaManager.GetAllCategory());

                model.CreateIngredients();

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel data, IFormFile foto)
        {
            if (!ModelState.IsValid)
            {

                // ottengo la lista degli errori
                var errorMessages = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                // Verifica se uno degli errori riguarda il campo "foto"

                if (errorMessages.Count != 1 || foto == null || foto.Length == 0)
                {
                    data.Categories = PizzaManager.GetAllCategory();

                    data.CreateIngredients();

                    return View("Create", data);
                }
            }

            // Ottieni il nome del file immagine caricato dall'utente
            string imgFileName = Path.GetFileName(foto.FileName);

            string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            string imgPath = Path.Combine(imgFolderPath, imgFileName);

            data.Pizza.Foto = "~/img/" + imgFileName;

            // richiamo funzione di modifica
            bool result = PizzaManager.UpdatePizza(id, data.Pizza, data.SelectedIngredients);

            if (result == true)
                return RedirectToAction("Index");
            else
                return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {

            if (PizzaManager.DeletePizza(id))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
