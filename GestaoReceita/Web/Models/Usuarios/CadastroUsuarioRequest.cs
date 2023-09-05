using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Usuarios
{
    public class CadastroUsuarioRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
    }
}