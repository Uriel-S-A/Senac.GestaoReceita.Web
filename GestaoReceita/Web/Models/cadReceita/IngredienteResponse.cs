using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class IngredienteResponse
    {
        public int id { get; set; }
        public string nomeIngrediente { get; set; }
        public decimal precoIngrediente { get; set; }
        public int quantidadeUnidade { get; set; }
        public UnidadeMedidaRequeste unidadeMedida { get; set; }
    }
}