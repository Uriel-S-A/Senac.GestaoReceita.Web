using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Ingrediente
{
    public class DadosIngredienteRequest
    {
        public int id { get; set; }
        public string nomeIngrediente { get; set; }
        public decimal precoIngrediente { get; set; }
        public int quantidadeUnidade { get; set; }
        public int empresaId { get; set; }
        public DadosEmpresa empresa { get; set; }
        public int unidadeMedidaId { get; set; }
        public DadosUnidadeMedida unidadeMedida { get; set; }
    }
}