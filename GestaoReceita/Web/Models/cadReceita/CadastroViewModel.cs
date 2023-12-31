﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReceitaFrontEnd.Models
{
    public class CadastroViewModel
    {
        public List<DadosIngredientes> DadosIngredientes { get; set; }

        public List<DadosIngredientes> DadosIngredientesDaReceita { get; set; }
        
        public string NomeReceita { get; set; }

        public int? Id { get; set; }
    }
}