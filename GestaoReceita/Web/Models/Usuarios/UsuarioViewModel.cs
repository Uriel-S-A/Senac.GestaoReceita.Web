using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Usuarios
{
    public class UsuarioViewModel
    {
        public string Nome { get; set; }
        public string Username { get; set; }
        public List<DadosUsuarioViewModel> ListaUsuarios {  get; set; } 

    }
}