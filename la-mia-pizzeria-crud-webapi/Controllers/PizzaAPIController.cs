using la_mia_pizzeria_crud_webapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Xml.Linq;


namespace la_mia_pizzeria_crud_webapi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaAPIController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllPizzas()
        {
            var Pizza = PizzaManager.GetAllPizza();

            return Ok(Pizza);
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var Category = PizzaManager.GetAllCategory();

            return Ok(Category);
        }

        [HttpGet]
        public IActionResult GetPizzaById(int id) // esempio--https://localhost:7077/api/PizzaAPI/GetPizzaByName/GetPizzaById?id=1
        {
            var Pizza = PizzaManager.GetPizza(id);
            if (Pizza == null)
                return NotFound("ERRORE");
            return Ok(Pizza);
        }

        [HttpGet("{name}")] // esempio--https://localhost:7077/api/PizzaAPI/GetPizzaByName/Margherita
        public IActionResult GetPizzaByName(string name)
        {
            var Pizza = PizzaManager.GetPizzaByName(name);
            if (Pizza == null)
                return NotFound("ERRORE");
            return Ok(Pizza);
        }

        /*[HttpPost]
        // Post deve essere incluso nella richiesta POST
        // (come documento JSON che il framework deserializzerà automaticamente in oggetto di tipo Post)
        public async Task<IActionResult> CreatePizza([FromBody] Pizza Pizza, IFormFile Foto)
        {
            
            string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
            string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(Foto.FileName);
            string imgPath = Path.Combine(imgFolderPath, imgFileName);

            using (var stream = new FileStream(imgPath, FileMode.Create))
            {
                await Foto.CopyToAsync(stream);
            }

            Pizza.Foto = "~/img/" + imgFileName;

            PizzaManager.InsertPizza(Pizza, null);
            return Ok();
        }*/

        [HttpPost]
        public async Task<IActionResult> CreatePizza([FromForm] Pizza pizza, [FromForm] IFormFile FotoIfile)
        {
            if (FotoIfile != null && FotoIfile.Length > 0)
            {
                // Definisci il percorso in cui salvare il file
                string imgFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");
                string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(FotoIfile.FileName);
                string imgPath = Path.Combine(imgFolderPath, imgFileName);

                // Assicurati che la directory esista
                Directory.CreateDirectory(imgFolderPath);

                // Salva il file nel percorso specificato
                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await FotoIfile.CopyToAsync(stream);
                }

                // Aggiorna la proprietà Foto dell'oggetto Pizza
                pizza.Foto += imgFileName;
            }

            // inseriSCO l'oggetto Pizza nel database 
            PizzaManager.InsertPizza(pizza, null);

            return Ok(new { Messaggio = "Pizza creata con successo" });
        }

        // restituire la lista di tutte le nostre pizze
        // (deve essere possibile passare un parametro di filtro e restituire le pizze il cui titolo contiene il filtro inviato)
        [HttpPut("{id}")]
        public IActionResult UpdatePizza(int id, [FromBody] Pizza Pizza)
        {
            var oldPizza = PizzaManager.GetPizza(id);
            if (oldPizza == null)
                return NotFound("ERRORE");
            PizzaManager.UpdatePizza(id, Pizza, null);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            bool found = PizzaManager.DeletePizza(id);
            if (found)
                return Ok();
            return NotFound();
        }

    }
}
