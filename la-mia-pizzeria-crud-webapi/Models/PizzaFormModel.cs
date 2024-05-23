using Microsoft.AspNetCore.Mvc.Rendering;

namespace la_mia_pizzeria_crud_webapi.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }

        public List<Category>? Categories { get; set; }


        //INGREDIENTI
        public List<SelectListItem>? Ingredients { get; set; } // Gli elementi degli ingredienti selezionabili per la select (analogo a Categories)

        public List<string>? SelectedIngredients { get; set; } // Gli ID degli elementi (ingredienti) effettivamente selezionati

        public PizzaFormModel() { }

        public PizzaFormModel(Pizza p, List<Category> c)
        {
            this.Pizza = p;
            this.Categories = c;
        }

        public void CreateIngredients()
        {
            this.Ingredients = new List<SelectListItem>();

            if (this.SelectedIngredients == null)
                this.SelectedIngredients = new List<string>();

            var IngredientsFromDB = PizzaManager.GetAllIngredient();

            foreach (var Ingrediente in IngredientsFromDB) // es. ingrediente1, ingrediente2, ingrediente3... ingrediente10
            {
                bool isSelected = this.SelectedIngredients.Contains(Ingrediente.id.ToString()); // this.Pizza.Ingredients?.Any(i => i.Id == ingrendient.Id) == true;


                this.Ingredients.Add(new SelectListItem() // lista degli elementi selezionabili
                {
                    Text = $"{Ingrediente.Nome} (Quantità: {Ingrediente.Quantita})",  // Nome visualizzato con quantità
                    Value = Ingrediente.id.ToString(), // SelectListItem vuole una generica stringa, non un int
                    Selected = isSelected // es. tag1, tag5, tag9
                });

            }
        }
    }
}
