using System;
using Web.Models.Pais;

namespace Web.Models.Estado
{
    public class EstadoViewModel
    {
        public int id { get; set; }
        public string descricaoEstado { get; set; }
        public int idPais { get; set; }
        public PaisViewModel pais { get; set; }

        public static explicit operator EstadoViewModel(EstadoTO v)
        {
            if (v == null) return null;

            return new EstadoViewModel
            {
                id = v.id,
                descricaoEstado = v.descricaoEstado,
                idPais = v.idPais,
                pais = (PaisViewModel)v.pais //ver sobre
            };
        }
    }
}