using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DadosIngrediente
    {
        public int Id { get; set; }

        public string NomeIngrediente { get; set; }

        public decimal PrecoIngrediente { get; set; }

        public float QuantidadeUnidade { get; set; }

        public int EmpresaId { get; set; }

        public int UnidadeMedidaId { get; set; }
    }
}