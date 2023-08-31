using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Web.Models.Cidade;
using Web.Models.Estado;

namespace Web.Controllers
{
    public class CidadeController : Controller
    {
        // GET: Estado
        public ActionResult Index()
        {
            List<CidadeViewModel> minhaLista = getCidades();
            return View(minhaLista);
        }

        public List<CidadeViewModel> getCidades()
        {
            List<CidadeViewModel> cidadeViewModel = new List<CidadeViewModel>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Cidades");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var cidadeToList = JsonConvert.DeserializeObject<List<CidadeTO>>(stringResult.Result);
                    List<CidadeViewModel> CidadeViewModelList = new List<CidadeViewModel>();

                    foreach (var cidadeTO in cidadeToList)
                    {
                        CidadeViewModel cidadevm = new CidadeViewModel
                        {
                            id = cidadeTO.id,
                            descricaoCidade = cidadeTO.descricaoCidade,
                            idEstado = cidadeTO.idEstado,
                            estado = (EstadoViewModel)cidadeTO.estado //ver sobre

                        };

                        CidadeViewModelList.Add(cidadevm);
                    }

                    return cidadeViewModel = CidadeViewModelList;
                }
                else
                {
                    //mensagem de erro de validação

                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);

                }
            }

            return cidadeViewModel;
        }

        public ActionResult AdicionarCidade(CidadeViewModel novaCidade)
        {
            if (ModelState.IsValid)
            {
                List<CidadeViewModel> minhaLista = getCidades();

                //comparação será sensível a maiúsculas e minúsculas usando StringComparison.OrdinalIgnoreCase
                bool cidadeJaExiste = minhaLista.Any(p => p.descricaoCidade.Equals(novaCidade.descricaoCidade, StringComparison.OrdinalIgnoreCase));

                if (cidadeJaExiste)
                {
                    //não aparece na tela
                    ModelState.AddModelError("descrição cidade", "A cidade já existe na lista.");
                    List<CidadeViewModel> lista = getCidades();
                    return View("Index", lista);
                }

                int novoId = minhaLista.Max(max => max.id) + 1;

                using (var client = new HttpClient())
                {
                    //idPais precisa vir preenchido quando eviado pela view model, deixei fixo

                    var formContentString = new StringContent(JsonConvert.SerializeObject(new { idEstado = 1, descricaoCidade = novaCidade.descricaoCidade, id = novoId }), Encoding.UTF8, "application/json");

                    var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Cidades", formContentString);

                    response.Wait();

                    if (response.Result.IsSuccessStatusCode)
                    {
                        // adicionar mensagem de sucesso

                        var stringResult = response.Result.Content.ReadAsStringAsync();
                        var objectJson = JsonConvert.DeserializeObject<CidadeTO>(stringResult.Result);
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

        public ActionResult EditarCidade(CidadeViewModel cidadeEditar)
        {
            //já chegando id e descrição do pais.

            var dados = getCidadeById(cidadeEditar.id);

            //dados
            //var Id = dados.id = estadoEditar.id;
            //var DescricaoPais = dados.descricaoPais = estadoEditar.descricaoPais;

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { idEstado = 1, descricaoCidade = cidadeEditar.descricaoCidade, id = cidadeEditar.id }), Encoding.UTF8, "application/json");

                var response = client.PutAsync(new Uri("http://gestaoreceitaapi.somee.com/api/Cidades/" + dados.id), formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    // adicionar mensagem de sucesso

                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<CidadeTO>(stringResult.Result);
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

        public CidadeViewModel getCidadeById(int id)
        {
            CidadeViewModel cidadeViewModel = new CidadeViewModel();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Cidades/" + id);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var cidadeTO = JsonConvert.DeserializeObject<CidadeTO>(stringResult.Result);

                    CidadeViewModel estadoView = (CidadeViewModel)cidadeTO;

                    return cidadeViewModel = estadoView;
                }
                else
                {
                    //tratar erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }

            return cidadeViewModel;
        }



        public ActionResult DeletarEstado(CidadeViewModel cidadeDeletar)
        {
            return Json(new { });
        }
    }
}