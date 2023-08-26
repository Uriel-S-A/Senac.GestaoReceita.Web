using System.Web.Mvc;
using Web.Model.CadastroUnidadeMedida;
using System.Collections.Generic;

namespace Web.Controllers
{
    public class CadastroUnidadeMedidaController : Controller
    {
        // GET: CadastroUnidadeMedida
        public ActionResult Index()
        {
            //chamada da api
            var listaUnidadeMedida = new List<UnidadeMedida>()
            {
                new UnidadeMedida()
                {
                    Id = 1,
                    Descricao = "Kilo",
                    Sigla = "Kg"
                }   ,            
                new UnidadeMedida()
                {
                    Id = 2,
                    Descricao = "Litros",
                    Sigla = "Lt"
                }  ,             
                new UnidadeMedida()
                {
                    Id = 1,
                    Descricao = "Kilo",
                    Sigla = "Kg"
                } ,              
                new UnidadeMedida()
                {
                    Id = 1,
                    Descricao = "Kilo",
                    Sigla = "Kg"
                }
            };

            var indexViewModel = new CadastrarUnidadeMedidaViewModel()
            {
                listaUnidadeMedida = listaUnidadeMedida
            };
            
            return View(indexViewModel);
        }

        public ActionResult PersistirUnidadeMedida(int? id, string descricao, string sigla)
        {


            return RedirectToAction("Index");
        }
    }
}