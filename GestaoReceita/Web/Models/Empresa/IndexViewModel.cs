using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Empresa
{
    public class IndexViewModel
    {
        public string inputCNPJ { get; set; }
        public List<CadastroEmpresaViewModel> listaEmpresa { get; set; } = new List<CadastroEmpresaViewModel>();

        public string mensagemErro { get; set; }
        public string mensagemSucesso { get; set; }
    }
}