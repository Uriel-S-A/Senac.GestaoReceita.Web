using SolutionWebCadastroLogin.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWebCadastroLogin.Models.UsuariosCadastro
{
    public class DadosUsuarioViewModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string username { get; set; }
        public string senha { get; set; }
        public int acesso { get; set; } = 1;
        public int manterLogado { get; set; }
        public int empresaId { get; set; }
        public int ativo { get; set; }

        public List<Empresa> listaEmpresas { get; set; } 

        public string mensagemErro { get; set; }

        public string mensagemSucesso { get; set; }
    }
}