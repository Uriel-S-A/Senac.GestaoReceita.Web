using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class IndexViewModel
    {
        public List<DadosIngrediente> listaIngredientesCadastrados { get; set; }
        public string erros { get; set; }
    }
}