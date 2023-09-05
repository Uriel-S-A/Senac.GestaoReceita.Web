using Web.Models.Pais;

namespace Web.Models.Estado
{
    public class EstadoTO
    {
        public int id { get; set; }
        public string descricaoEstado { get; set; }
        public int idPais { get; set; }
        public PaisTO pais { get; set; }
    }
}