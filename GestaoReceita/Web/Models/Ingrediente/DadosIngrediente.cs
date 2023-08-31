using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web.Models.Ingrediente;

namespace Web.Models
{
    public class DadosIngrediente
    {
        public int Id { get; set; }

        public string NomeIngrediente { get; set; }

        public string PrecoIngrediente { get; set; }

        public float QuantidadeUnidade { get; set; }

        public int EmpresaId { get; set; }

        public DadosEmpresa Empresa { get; set; }

        public int UnidadeMedidaId { get; set; }

        public DadosUnidadeMedida UnidadeMedida { get; set; }
    }
}