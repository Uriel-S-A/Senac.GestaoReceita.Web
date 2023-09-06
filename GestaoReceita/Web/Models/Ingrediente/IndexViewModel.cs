using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Ingrediente;

namespace Web.Models
{
    public class IndexViewModel
    {
        public List<DadosIngrediente> listaIngredientesCadastrados { get; set; }
        public List<DadosEmpresa> listaDadosEmpresa { get; set; }
        public List<DadosUnidadeMedida> listaDadosUnidadeMedida { get; set; }
        public string sucessoMensagem { get; set; }
        public string erroMensagem { get; set; }

        //public List<string> listaErros { get; set; }
    }
}