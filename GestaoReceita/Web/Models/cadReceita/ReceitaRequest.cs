using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class ReceitaRequest
    {
        public int id {get;set;}
        public string nomeReceita { get; set; }
        public string modoPreparo { get; set; }
        public decimal valorTotalReceita { get; set; }
        public List<ReceitaIngredienteResponse> receitaIngrediente { get; set; }
        
    }
}