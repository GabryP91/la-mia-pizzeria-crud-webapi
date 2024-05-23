using la_mia_pizzeria_crud_webapi.Context;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_crud_webapi.Models
{
    public class PizzaManager
    {


        //funzione per verificare che la tabella pizza sia popolata
        public static int CountAllPizzas()
        {
            using (PizzaContext db = new PizzaContext())
            {
                return db.Pizza.Count();
            }

        }



        //funzione per ottenere tutti dati nella tabella pizza
        public static List<Pizza> GetAllPizza(bool includeCategories = false)
        {
            using (PizzaContext db = new PizzaContext())
            {
                if (includeCategories) return db.Pizza.Include(c => c.Category).Include(i => i.Ingredients).ToList();

                else return db.Pizza.ToList();
            }

        }

        //funzione per ottenere tutti dati nella tabella categoria
        public static List<Category> GetAllCategory()
        {
            using (PizzaContext db = new PizzaContext())
            {
                return db.Category.ToList();
            }

        }

        //funzione per ottenere tutti dati nella tabella categoria
        public static List<Ingredient> GetAllIngredient()
        {
            using (PizzaContext db = new PizzaContext())
            {
                return db.Ingrediente.ToList();
            }

        }


        //funzione per Richiamare la pizza tramite id
        public static Pizza GetPizza(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                //restituiscimi la prima pizza con id uguale a quello passato con tutte la categoria
                Pizza pizza = db.Pizza
                    .Where(p => p.id == id)
                    .Include(c => c.Category)
                    .Include(i => i.Ingredients)
                    .FirstOrDefault();

                return pizza;
            }

        }


        //funzione inserimetno singolo ingrediente
        public static void InsertIngredient(Ingredient ingrediente)
        {
            using (PizzaContext db = new PizzaContext())
            {

                db.Ingrediente.Add(ingrediente);
                db.SaveChanges();
            }

        }

        //funzione inserimetno singola pizza
        public static void InsertPizza(Pizza pizza, List<string> SelectedIngredients = null)
        {
            using (PizzaContext db = new PizzaContext())
            {
                if (SelectedIngredients != null)
                {
                    pizza.Ingredients = new List<Ingredient>();

                    // Trasformiamo gli ID scelti in Ingredient da aggiungere tra i riferimenti in Pizza
                    foreach (var IngredientsId in SelectedIngredients)
                    {
                        int id = int.Parse(IngredientsId);
                        var ingredient = db.Ingrediente.FirstOrDefault(t => t.id == id); // PostManager.GetTagById(id); NON usiamo GetTagById() perché usa un db context diverso e ciò causerebbe errore in fase di salvataggio - usiamo lo stesso context all'interno della stessa operazione
                        pizza.Ingredients.Add(ingredient);
                    }
                }

                db.Pizza.Add(pizza);
                db.SaveChanges();
            }

        }

        public static void SeedIng()
        {
            using (PizzaContext db = new PizzaContext())
            {
                if (PizzaManager.GetAllIngredient() == 0)
                { 
                        var ingredienti = new List<Ingredient>
                    {
                        new Ingredient {Nome = "Mozzarella", Quantita = 15 },
                        new Ingredient {Nome = "Pomodoro", Quantita = 50 },
                        new Ingredient {Nome = "Basilico", Quantita = 5 },
                        new Ingredient {Nome = "SalamePiccante", Quantita = 15 },
                        new Ingredient {Nome = "Rucola", Quantita = 25 },
                        new Ingredient {Nome = "Parmigiano", Quantita = 15 },
                        new Ingredient {Nome = "Zucchina", Quantita = 20 },
                        new Ingredient {Nome = "Melanzana", Quantita = 1 },
                        new Ingredient {Nome = "Bresaola", Quantita = 10 },
                        new Ingredient {Nome = "FungoPorcino", Quantita = 20 }
                    };

                    // Verifica se l'ingrediente esiste già, altrimenti lo aggiunge
                    foreach (var ingrediente in ingredienti)
                    {

                        db.Ingrediente.Add(ingrediente);
                    }

                    db.SaveChanges();
                
                }

            }
        }


        //funzione inserimetno singola pizza
        public static void Seed()
        {
            using (PizzaContext db = new PizzaContext())
            {
                if (PizzaManager.CountAllPizzas() == 0)
                {

                    // Recupera gli ingredienti necessari dal database
                var mozzarella = db.Ingrediente.FirstOrDefault(i => i.Nome == "Mozzarella")?.id.ToString();
                var pomodoro = db.Ingrediente.FirstOrDefault(i => i.Nome == "Pomodoro")?.id.ToString();
                var basilico = db.Ingrediente.FirstOrDefault(i => i.Nome == "Basilico")?.id.ToString();
                var salameP = db.Ingrediente.FirstOrDefault(i => i.Nome == "SalamePiccante")?.id.ToString();
                var rucola = db.Ingrediente.FirstOrDefault(i => i.Nome == "Rucola")?.id.ToString();
                var parmigiano = db.Ingrediente.FirstOrDefault(i => i.Nome == "Parmigiano")?.id.ToString();
                var zucchina = db.Ingrediente.FirstOrDefault(i => i.Nome == "Zucchina")?.id.ToString();
                var melanzana = db.Ingrediente.FirstOrDefault(i => i.Nome == "Melanzana")?.id.ToString();
                var bresaola = db.Ingrediente.FirstOrDefault(i => i.Nome == "Bresaola")?.id.ToString();
                var fungoP = db.Ingrediente.FirstOrDefault(i => i.Nome == "FungoPorcino")?.id.ToString();

                // Crea una lista di stringhe con gli ID degli ingredienti per ogni pizza

                var ingredientsMargherita = new List<String>
                 {
                      mozzarella,
                      pomodoro,
                      basilico

                 };

                var ingredientsDiavola = new List<String>
                 {
                     mozzarella,
                     pomodoro,
                     salameP
                 };

                var ingredientsCrudaiola = new List<String>
                 {
                     mozzarella,
                     pomodoro,
                     rucola,
                     parmigiano
                 };

                var ingredientsOrtolana = new List<String>
                 {
                     mozzarella,
                     pomodoro,
                     rucola,
                     zucchina,
                     melanzana
                 };

                var ingredientsSfiziosa = new List<String>
                 {
                      mozzarella,
                      pomodoro,
                      rucola,
                      bresaola
                 };

                var ingredientsPorcina = new List<String>
                 {
                      mozzarella,
                      pomodoro,
                      basilico,
                      fungoP
                 };




                

                    PizzaManager.InsertPizza(new Pizza("Margherita", "Molto buona", "~/img/margherita.jpg", 8, 1), ingredientsMargherita);
                    PizzaManager.InsertPizza(new Pizza("Diavola", "buona", "~/img/Diavola.jpg", 10.5f, 1), ingredientsDiavola);
                    PizzaManager.InsertPizza(new Pizza("Ortolana", "ottima", "~/img/Ortolana.jpg", 8.7f, 3), ingredientsOrtolana);
                    PizzaManager.InsertPizza(new Pizza("Crudaiola", "discreta", "~/img/Crudaiola.jpg", 11, 1), ingredientsCrudaiola);
                    PizzaManager.InsertPizza(new Pizza("Sfiziosa", "buona", "~/img/Sfiziosa.jpg", 9.4f, 3), ingredientsSfiziosa);
                    PizzaManager.InsertPizza(new Pizza("Porcina", "pessima", "~/img/Porcina.jpg", 6, 2), ingredientsPorcina);


                    db.SaveChanges();

                }

            }
        }


        public static bool UpdatePizza(int id, Pizza pizza, List<string> selectedIngredients)
        {

            using PizzaContext db = new PizzaContext();

            //ricerca e restituisce la prima posizione con lo stesso id passato

            var pizzaDaModificare = db.Pizza.Where(p => p.id == id).Include(p => p.Ingredients).FirstOrDefault();

            if (pizzaDaModificare == null)
                return false;

            pizzaDaModificare.Nome = pizza.Nome;
            pizzaDaModificare.Descrizione = pizza.Descrizione;
            pizzaDaModificare.Prezzo = pizza.Prezzo;
            pizzaDaModificare.Foto = pizza.Foto;
            pizzaDaModificare.Categoryid = pizza.Categoryid;

            // Prima svuoto così da salvare solo le informazioni che l'utente ha scelto, NON le aggiungiamo ai vecchi dati
            pizzaDaModificare.Ingredients.Clear();

            if (selectedIngredients != null)
            {
                foreach (var ingredient in selectedIngredients)
                {
                    int ingredientId = int.Parse(ingredient);
                    var ingredientFromDb = db.Ingrediente.FirstOrDefault(x => x.id == ingredientId);
                    if (ingredientFromDb != null)
                        pizzaDaModificare.Ingredients.Add(ingredientFromDb);
                }
            }

            db.SaveChanges();

            return true;
        }

        public static bool DeletePizza(int id)
        {
            using PizzaContext db = new PizzaContext();


            var pizza = db.Pizza.FirstOrDefault(p => p.id == id);

            if (pizza == null)
                return false;

            db.Pizza.Remove(pizza);

            db.SaveChanges();

            return true;
        }

        public static void DeleteAllPizza()
        {
            using PizzaContext db = new PizzaContext();


            foreach (Pizza pizza in db.Pizza.ToList())
            {
                db.Pizza.Remove(pizza);
            }

            foreach (Ingredient ingrediente in db.Ingrediente.ToList())
            {
                db.Ingrediente.Remove(ingrediente);
            }

            db.SaveChanges();

        }
    }
}
