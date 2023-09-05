using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Estado;
using Web.Models.Pais;

namespace Web.Models.Cidade
{
    public class CidadeViewModel
    {
        public int id { get; set; }
        public string descricaoCidade { get; set; }
        public int idEstado { get; set; }
        public EstadoViewModel estado { get; set; }

        public string descricaoEstado { get; set; }
        public List<EstadoViewModel> listaEstados { get; set; }

        public static explicit operator CidadeViewModel(CidadeTO v)
        {
            if (v == null) return null;

            return new CidadeViewModel
            {
                id = v.id,
                descricaoCidade = v.descricaoCidade,
                idEstado = v.idEstado,
                estado = (EstadoViewModel)v.estado 
            };
        }
    }
}