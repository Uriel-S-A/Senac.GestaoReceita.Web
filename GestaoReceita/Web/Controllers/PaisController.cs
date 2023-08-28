using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Web.Models.Pais;

namespace Web.Controllers
{
    public class PaisController : Controller
    {
        //rever alguns nomes e deixar padronizado!

        public ActionResult Index()
        {
            List<PaisViewModel> minhaLista = ObterListaPaises();
            return View(minhaLista);
        }

        private void SalvarListaPaises(List<PaisViewModel> lista)
        {
            Session["ListaPaises"] = lista;
        }

        private List<PaisViewModel> ObterListaPaises()
        {
            var lista = Session["ListaPaises"] as List<PaisViewModel>;

            if (lista == null)
            {
                lista = new List<PaisViewModel>
                {
                    new PaisViewModel { Id = 1, PaisNome = "Brasil", PaisSigla = "BRA" },
                    new PaisViewModel { Id = 2, PaisNome = "Estados Unidos", PaisSigla = "EUA" }
                };

                SalvarListaPaises(lista);
            }
            return lista;
        }

        public ActionResult AdicionarNovoPais(PaisViewModel novoPais)
        {
            if (ModelState.IsValid)
            {
                List<PaisViewModel> minhaLista = ObterListaPaises();
                int novoId = minhaLista.Max(max => max.Id) + 1;

                novoPais.Id = novoId;
                novoPais.PaisNome = novoPais.PaisNome;
                novoPais.PaisSigla = novoPais.PaisSigla;
                minhaLista.Add(novoPais);

                SalvarListaPaises(minhaLista);

                return RedirectToAction("Index");
            }

            return View("Index", ObterListaPaises());
        }
          
        public ActionResult EditarPais(PaisViewModel paisEditar)
        {
            if (ModelState.IsValid)
            {
                List<PaisViewModel> listaPaises = ObterListaPaises();

                var paisToUpdate = listaPaises.FirstOrDefault(fd => fd.Id == paisEditar.Id);

                if (paisToUpdate != null)
                {
                    paisToUpdate.PaisNome = paisEditar.PaisNome;
                    paisToUpdate.PaisSigla = paisEditar.PaisSigla;

                    // Salva a lista atualizada na session
                    SalvarListaPaises(listaPaises);

                    return RedirectToAction("Index");
                }
            }

            return View("Index", ObterListaPaises());
        }

        public ActionResult DeletarPais(PaisViewModel paisDeletar)
        {
            //precisa haver uma validação se o pais que está sendo deletado
            //possui algum vinculo com algum estado ou cidade

            //fazendo sem a validação por enquanto

            if (ModelState.IsValid)
            {
                List<PaisViewModel> listaPaises = ObterListaPaises();

                var paisToDelete = listaPaises.FirstOrDefault(fd => fd.Id == paisDeletar.Id);

                if(paisToDelete != null)
                {
                    listaPaises.Remove(paisToDelete);

                    SalvarListaPaises(listaPaises);

                    return RedirectToAction("Index");
                }                
            }

            return View("Index", ObterListaPaises());
        }
    }
}