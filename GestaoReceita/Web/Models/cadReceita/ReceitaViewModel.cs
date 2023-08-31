using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class ReceitaViewModel
    {
        public int Id { get; set; }
        public double valorTotalReceita { get; set; }
        public string nomeReceita { get; set; }
        
    }
}