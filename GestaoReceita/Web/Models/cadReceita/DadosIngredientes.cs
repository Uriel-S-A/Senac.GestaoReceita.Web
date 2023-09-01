using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class DadosIngredientes
    {
        public int Id { get; set; }
        public string NomeIngrediente { get; set; }
        public decimal PrecoIngrediente { get; set; }
        public string UnidadeMedida { get; set; }

        public int Quantidade { get; set; }

        public string NomeReceita { get; set; }
        public int? IdReceita { get; set; }
    }
}