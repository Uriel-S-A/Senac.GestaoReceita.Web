using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class UnidadeMedidaRequeste
    {
        public int id { get; set; }
        public string descUnidMedIngrediente { get; set; }
        public string sigla { get; set; }
    }
}