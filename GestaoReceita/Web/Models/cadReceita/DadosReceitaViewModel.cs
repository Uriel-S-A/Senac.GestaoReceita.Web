using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class DadosReceitaViewModel
    {
        public int Id { get; set; }
        public double valorTotalReceita { get; set; }
        public string nomeReceita { get; set; }
    }
}