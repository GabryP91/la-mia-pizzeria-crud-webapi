namespace la_mia_pizzeria_crud_webapi.Models
{
    public class Ingredient
    {
        public int id { get; set; }

        public string Nome { get; set; }

        public float Quantita { get; set; }

        public List<Pizza>? Pizzas { get; set; }

    }
}
