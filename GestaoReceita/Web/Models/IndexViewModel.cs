using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CadEmpresa.Models
{
    public class IndexViewModel
    {
        public string inputCNPJ { get; set; }
        public List<CadastroEmpresaViewModel> listaEmpresa { get; set; } = new List<CadastroEmpresaViewModel>();

        public string erro { get; set; }
    }
}