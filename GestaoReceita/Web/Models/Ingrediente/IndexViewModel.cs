using System.Collections.Generic;

namespace Web.Models.Ingrediente
{
    public class IndexViewModel
    {
        public List<DadosIngrediente> listaIngredientesCadastrados { get; set; }
        public List<DadosEmpresa> listaDadosEmpresa { get; set; }
        public List<DadosUnidadeMedida> listaDadosUnidadeMedida { get; set; }

        //public string erros { get; set; }
        //public List<string> listaErros { get; set; }
    }
}