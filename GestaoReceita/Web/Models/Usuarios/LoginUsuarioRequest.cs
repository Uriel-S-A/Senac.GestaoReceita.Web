using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionWebCadastroLogin.Models.Usuarios
{
    public class LoginUsuarioRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nome { get; set; }
    }
}