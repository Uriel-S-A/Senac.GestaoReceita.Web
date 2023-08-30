using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class DadosIngrediente
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        [UIHint("inputNumerico")]
        public string NomeIngrediente { get; set; }

        [DisplayName("Preço Ingrediente")]
        [UIHint("inputNumerico")]
        public decimal PrecoIngrediente { get; set; }

        public float QuantidadeUnidade { get; set; }

        public int EmpresaId { get; set; }

        public int UnidadeMedidaId { get; set; }
    }
}