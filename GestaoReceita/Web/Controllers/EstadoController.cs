using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.Pais;
using Web.Models.Estado;    

namespace Web.Controllers
{
    public class EstadoController : Controller
    {
        // GET: Estado
        public ActionResult Index()
        {
            List<EstadoViewModel> minhaLista = ObterListaEstados();
            return View(minhaLista);
        }
        private void SalvarListaEstados(List<EstadoViewModel> lista)
        {
            Session["ListaEstados"] = lista;
        }

        private List<EstadoViewModel> ObterListaEstados()
        {
            var lista = Session["ListaEstados"] as List<EstadoViewModel>;

            if (lista == null)
            {
                lista = new List<EstadoViewModel>
                {
                    new EstadoViewModel { Id = 1, Pais = "Brasil", Estado = "Rio Grande do Sul" ,Sigla = "BRA" },
                    new EstadoViewModel { Id = 2, Pais = "Estados Unidos", Estado = "Rio de Janeiro" ,Sigla = "EUA" }
                };

                SalvarListaEstados(lista);
            }
            return lista;
        }

        public ActionResult AdicionarNovoEstado(EstadoViewModel novoEstado)
        {
            if (ModelState.IsValid)
            {
                List<EstadoViewModel> minhaLista = ObterListaEstados();
                int novoId = minhaLista.Max(max => max.Id) + 1;

                novoEstado.Id = novoId;
                novoEstado.Pais = novoEstado.Pais;
                novoEstado.Estado = novoEstado.Estado;
                novoEstado.Sigla = novoEstado.Sigla;
                minhaLista.Add(novoEstado);

                SalvarListaEstados(minhaLista);

                return RedirectToAction("Index");
            }

            return View("Index", ObterListaEstados());
        }

        public ActionResult EditarEstado(EstadoViewModel EstadoEditar)
        {
            //if (ModelState.IsValid)
            //{
                List<EstadoViewModel> listaEstados = ObterListaEstados();

                var estadoToUpdate = listaEstados.FirstOrDefault(fd => fd.Id == EstadoEditar.Id);

                if (estadoToUpdate != null)
                {
                    estadoToUpdate.Pais = EstadoEditar.Pais;
                    estadoToUpdate.Estado = EstadoEditar.Estado;
                    estadoToUpdate.Sigla = EstadoEditar.Sigla;

                    // Salva a lista atualizada na session
                    SalvarListaEstados(listaEstados);

                    return RedirectToAction("Index");
                }
            //}

            return View("Index", ObterListaEstados());
        }

        public ActionResult DeletarEstado(EstadoViewModel EstadoDeletar)
        {
            //precisa haver uma validação se o pais que está sendo deletado
            //possui algum vinculo com algum estado ou cidade

            //fazendo sem a validação por enquanto

            if (ModelState.IsValid)
            {
                List<EstadoViewModel> listaEstados = ObterListaEstados();

                var estadoToDelete = listaEstados.FirstOrDefault(fd => fd.Id == EstadoDeletar.Id);

                if (estadoToDelete != null)
                {
                    listaEstados.Remove(estadoToDelete);

                    SalvarListaEstados(listaEstados);

                    return RedirectToAction("Index");
                }
            }

            return View("Index", ObterListaEstados());
        }
    }
}