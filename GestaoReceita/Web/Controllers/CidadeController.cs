using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Web.Models.Cidade;
using Web.Models.Estado;

namespace Web.Controllers
{
    public class CidadeController : LoginController
    {
        // GET: Estado
        public ActionResult Index()
        {

            try
            {
                List<CidadeViewModel> minhaLista = getCidades();
                return View(minhaLista);
            }
            catch (HttpRequestExceptionEx ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public class HttpRequestExceptionEx : Exception
        {
            public HttpRequestExceptionEx(string message) : base(message) { }
        }

        public List<CidadeViewModel> getCidades()
        {
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
                            estado = (EstadoViewModel)cidadeTO.estado

                        };

                        CidadeViewModelList.Add(cidadevm);
                    }

                    return CidadeViewModelList;
                }
                else
                {
                    throw new HttpRequestExceptionEx("Erro ao pegar dados de Cidade: " + response.Result.ReasonPhrase);
                }
            }
        }

        public ActionResult AdicionarCidade(CidadeViewModel novaCidade)
        {
            List<CidadeViewModel> minhaLista = getCidades();
             
            if (ModelState.IsValid)
            {                                
                bool cidadeJaExiste = minhaLista.Any(p => p.descricaoCidade.Equals(novaCidade.descricaoCidade, StringComparison.OrdinalIgnoreCase));

                if (cidadeJaExiste)
                {
                    ModelState.AddModelError("cidadeJaExiste", "Esta cidade já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }

                List<EstadoViewModel> listaEstados = this.getEstados();

                novaCidade.idEstado = 0;

                foreach (var meuEstado in listaEstados)
                {
                    if (meuEstado.descricaoEstado == novaCidade.descricaoEstado)
                    {
                        novaCidade.idEstado = meuEstado.id;
                    }
                }

                int novoId = minhaLista.Max(max => max.id) + 1;

                this.CadastrarNovaCidade(novaCidade, novoId);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("erro", "Erro ao cadastrar.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
            }

            return PartialView("_CreateCidadePartial", minhaLista);
        }
        private void CadastrarNovaCidade(CidadeViewModel novaCidade, int novoId)
        {
            using (var client = new HttpClient())
            {                
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { idEstado = novaCidade.idEstado, descricaoCidade = novaCidade.descricaoCidade, id = novoId }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Cidades", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {                    
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<CidadeTO>(stringResult.Result);                    
                }
                else
                {
                    throw new HttpRequestExceptionEx("Erro ao cadastrar nova Cidade: " + response.Result.ReasonPhrase);
                }
            }
        }


        public ActionResult EditarCidade(CidadeViewModel cidadeEditar)
        {
            var estados = getEstados();

            CidadeViewModel dadosCidade = getCidadeById(cidadeEditar.id);

            CidadeViewModel cidadeView = new CidadeViewModel()
            {
                id = dadosCidade.id,
                descricaoCidade = dadosCidade.descricaoCidade,
                idEstado = dadosCidade.idEstado,
                estado = dadosCidade.estado,
                listaEstados = estados
            };

            List<CidadeViewModel> minhaLista = getCidades();

            if (this.ModelState.IsValid)
            {
                bool cidadeJaExiste = minhaLista.Any(p => p.descricaoCidade.Equals(cidadeEditar.descricaoCidade, StringComparison.OrdinalIgnoreCase));

                if (cidadeJaExiste)
                {
                    ModelState.AddModelError("cidadeJaExiste", "Este estado já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }

                this.NovaEdicaoCidade(cidadeEditar, dadosCidade);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("error", "Erro ao editar cidade.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
            }

            return PartialView("_UpdateCidadePartial", cidadeView);
        }

        public void NovaEdicaoCidade(CidadeViewModel cidadeEditar, CidadeViewModel dadosCidade)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { idEstado = cidadeEditar.idEstado, descricaoCidade = cidadeEditar.descricaoCidade, id = cidadeEditar.id }), Encoding.UTF8, "application/json");

                var response = client.PutAsync(new Uri("http://gestaoreceitaapi.somee.com/api/Cidades/" + dadosCidade.id), formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<EstadoTO>(stringResult.Result);
                }
                else
                {
                    throw new HttpRequestExceptionEx("Erro ao fazer nova edição de Cidade: " + response.Result.ReasonPhrase);
                }
            }
        }

        public JsonResult DeletarCidade(int Id)
        {
            var retorno = "";

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Cidades/" + Id);

                response.Wait();

                retorno = "Cidade deletada com sucesso";

                if (!response.Result.IsSuccessStatusCode)
                {
                    retorno = "Erro: " + response.Result.ReasonPhrase;
                }
            }
            return Json(new { mensagemRetorno = retorno });
        }

        //Modal Create
        public ActionResult getModalEstados()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Estados");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var estadoTOList = JsonConvert.DeserializeObject<List<EstadoTO>>(stringResult.Result);
                    List<EstadoViewModel> estadoViewModelList = new List<EstadoViewModel>();

                    foreach (var estadoTO in estadoTOList)
                    {
                        EstadoViewModel estadovm = new EstadoViewModel
                        {
                            id = estadoTO.id,
                            descricaoEstado = estadoTO.descricaoEstado,
                        };

                        estadoViewModelList.Add(estadovm);
                    }

                    return PartialView("_CreateCidadePartial", estadoViewModelList);
                }
                else
                {
                    throw new HttpRequestExceptionEx("Erro ao pegar dados de Modal Creates: " + response.Result.ReasonPhrase);
                }
            }
        }

        //Modal Update
        public ActionResult getEstadosAndById(CidadeViewModel cidadeViewModel)
        {
            var estados = getEstados();

            CidadeViewModel CidadesDados = getCidadeById(cidadeViewModel.id);

            CidadeViewModel CiadadesView = new CidadeViewModel()
            {
                id = CidadesDados.id,
                descricaoCidade = CidadesDados.descricaoCidade,
                idEstado = CidadesDados.idEstado,
                estado = CidadesDados.estado,
                listaEstados = estados
            };

            return PartialView("_UpdateCidadePartial", CiadadesView);
        }

        //CONSULTAS:
        private List<EstadoViewModel> getEstados()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Estados");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var paisToList = JsonConvert.DeserializeObject<List<EstadoTO>>(stringResult.Result);
                    List<EstadoViewModel> estadoViewModelList = new List<EstadoViewModel>();

                    foreach (var estadoTO in paisToList)
                    {
                        EstadoViewModel estadovm = new EstadoViewModel
                        {
                            id = estadoTO.id,
                            descricaoEstado = estadoTO.descricaoEstado,
                        };

                        estadoViewModelList.Add(estadovm);
                    }

                    return estadoViewModelList;
                }
                else
                {
                    throw new HttpRequestExceptionEx("Erro ao pegar dados de Estado: " + response.Result.ReasonPhrase);
                }
            }
        }

        public CidadeViewModel getCidadeById(int id)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Cidades/" + id);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var cidadeTO = JsonConvert.DeserializeObject<CidadeTO>(stringResult.Result);

                    CidadeViewModel estadoView = (CidadeViewModel)cidadeTO;

                    return estadoView;
                }
                else
                { 
                    throw new HttpRequestExceptionEx("Erro ao pegar Cidade By id: " + response.Result.ReasonPhrase);
                }
            }
        }
    }
}