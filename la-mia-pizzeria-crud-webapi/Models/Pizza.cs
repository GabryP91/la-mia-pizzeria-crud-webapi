using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_crud_webapi.Models
{
    [Index(nameof(Nome), IsUnique = true)]
    public class Pizza
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il nome non può superare i 100 caratteri")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "La descrizione è obbligatoria")]
        [StringLength(255, ErrorMessage = "La descrizione non può superare i 255 caratteri")]
        //[MinWords(5, ErrorMessage = "La descrizione deve contenere almeno 5 parole")]
        public string Descrizione { get; set; }

        [Required(ErrorMessage = "La foto è obbligatoria")]
        public string Foto { get; set; }

        [Required(ErrorMessage = "Il prezzo è obbligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il prezzo deve essere maggiore di zero")]
        public float Prezzo { get; set; }
        // Foreign key (relazione 1 a n)
        public int Categoryid { get; set; }

        public Category? Category { get; set; }


        //relazione n a n

        public List<Ingredient>? Ingredients { get; set; }


        //METODI
        public Pizza() { }

        public Pizza(string nome, string descrizione, string foto, float prezzo, int id) : this()
        {
            this.Nome = nome;

            this.Descrizione = descrizione;

            this.Foto = foto;

            this.Prezzo = prezzo;

            this.Categoryid = id;


        }

        //override del metodo ToString
        public override string ToString()
        {
            return $"\nNome: {Nome} - Descrizione: {Descrizione} - Foto: {Foto} - Prezzo: {Prezzo} - Categoria: {(Category == null ? "Nessuna categoria" : Category.Nome)}";
        }

    }
}
