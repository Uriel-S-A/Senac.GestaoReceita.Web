using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Web.Models.Estado;
using Web.Models.Pais;

namespace Web.Controllers
{
    public class EstadoController : Controller
    {
        // GET: Estado
        public ActionResult Index()
        {
            List<EstadoViewModel> minhaLista = getEstados();
            return View(minhaLista);            
        }

        public List<EstadoViewModel> getEstados()
        {
            List<EstadoViewModel> estadoViewModel = new List<EstadoViewModel>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Estados");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var estadoToList = JsonConvert.DeserializeObject<List<EstadoTO>>(stringResult.Result);
                    List<EstadoViewModel> estadoViewModelList = new List<EstadoViewModel>();

                    foreach (var estadoTO in estadoToList)
                    {
                        EstadoViewModel estadovm = new EstadoViewModel
                        {
                            id = estadoTO.id,
                            descricaoEstado = estadoTO.descricaoEstado,
                            idPais = estadoTO.idPais,
                            pais = (PaisViewModel)estadoTO.pais //ver sobre

                        };

                        estadoViewModelList.Add(estadovm);
                    }

                    return estadoViewModel = estadoViewModelList;
                }
                else
                {
                    //mensagem de erro de validação

                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);

                }
            }

            return estadoViewModel;
        }

        public ActionResult AdicionarEstado(EstadoViewModel novoEstado)
        {
            if (ModelState.IsValid)
            {
                List<EstadoViewModel> minhaLista = getEstados();

                //comparação será sensível a maiúsculas e minúsculas usando StringComparison.OrdinalIgnoreCase
                bool estadoJaExiste = minhaLista.Any(p => p.descricaoEstado.Equals(novoEstado.descricaoEstado, StringComparison.OrdinalIgnoreCase));

                if (estadoJaExiste)
                {
                    //não aparece na tela
                    ModelState.AddModelError("descricaoPais", "O país já existe na lista.");
                    List<EstadoViewModel> lista = getEstados();
                    return View("Index", lista);
                }

                int novoId = minhaLista.Max(max => max.id) + 1;

                using (var client = new HttpClient())
                {
                    //idPais precisa vir preenchido quando eviado pela view model, deixei fixo

                    var formContentString = new StringContent(JsonConvert.SerializeObject(new { idPais = 1 , descricaoEstado = novoEstado.descricaoEstado, id = novoId }), Encoding.UTF8, "application/json");

                    var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Estados", formContentString);

                    response.Wait();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        // adicionar mensagem de sucesso

                        var stringResult = response.Result.Content.ReadAsStringAsync();
                        var objectJson = JsonConvert.DeserializeObject<EstadoTO>(stringResult.Result); 
                        //todas info que vem do banco devem ser armazenadas num TO.
                        //
                    }
                    else
                    {
                        // adicionar mensagem de erro
                        var content = response.Result.Content.ReadAsStringAsync();
                        var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                    }
                }
            }

            return Index();
        }

        public ActionResult EditarEstado(EstadoViewModel estadoEditar)
        {
            //já chegando id e descrição do pais.

            var dados = getEstadoById(estadoEditar.id);

            //dados
            //var Id = dados.id = estadoEditar.id;
            //var DescricaoPais = dados.descricaoPais = estadoEditar.descricaoPais;

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { idPais = 1, descricaoEstado = estadoEditar.descricaoEstado, id = estadoEditar.id}), Encoding.UTF8, "application/json");

                var response = client.PutAsync(new Uri("http://gestaoreceitaapi.somee.com/api/Estados/" + dados.id), formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    // adicionar mensagem de sucesso

                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<EstadoTO>(stringResult.Result);
                }
                else
                {
                    // adicionar mensagem de erro
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return Json(new { });
        }

        public EstadoViewModel getEstadoById(int id)
        {
            EstadoViewModel estadoViewModel = new EstadoViewModel();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Estados/" + id);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var estadoTO = JsonConvert.DeserializeObject<EstadoTO>(stringResult.Result);

                    EstadoViewModel paisView = (EstadoViewModel)estadoTO;

                    return estadoViewModel = paisView;
                }
                else
                {
                    //tratar erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return estadoViewModel;
        }



        public JsonResult DeletarEstado(int Id)
        {
            var retorno = "";

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Estados/" + Id);

                response.Wait();

                retorno = "Estado deletado com sucesso";

                if (!response.Result.IsSuccessStatusCode)
                {
                    retorno = "Erro: " + response.Result.ReasonPhrase;
                }
            }
            return Json(new { mensagemRetorno = retorno });
        }
    }
}