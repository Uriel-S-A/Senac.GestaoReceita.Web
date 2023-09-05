using SolutionWebCadastroLogin.Models.UsuariosCadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Usuario
{
    public class UsuarioViewModel
    {
        public string Nome { get; set; }
        public string Username { get; set; }
        public List<DadosUsuarioViewModel> ListaUsuarios {  get; set; } 

    }
}