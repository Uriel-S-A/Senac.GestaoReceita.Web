using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.CadastroUnidadeMedida
{
    public class UnidadeMedidaRequest
    {
        public int id { get; set; }
        public string descUnidMedIngrediente { get; set; }
        public string sigla { get; set; }
    }
}