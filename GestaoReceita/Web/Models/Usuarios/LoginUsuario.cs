using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWebCadastroLogin.Models
{
    public class LoginUsuario
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string mensagemErroAutenticacao { get; set; }
        public string mensagemSucesso { get; set; }

    }
}