using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class ReceitaIngredienteResponse
    {
        public int idingrediente { get; set; }
        public int quantidadeIngrediente { get; set; }
        public IngredienteResponse ingrediente { get; set; }
        
    }
}