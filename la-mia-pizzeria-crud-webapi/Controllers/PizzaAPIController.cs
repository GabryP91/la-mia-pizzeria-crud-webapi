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

        [HttpPost]
        // Post deve essere incluso nella richiesta POST
        // (come documento JSON che il framework deserializzerà automaticamente in oggetto di tipo Post)
        public IActionResult CreatePizza([FromBody] Pizza Pizza)
        {
            PizzaManager.InsertPizza(Pizza, null);
            return Ok();
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
