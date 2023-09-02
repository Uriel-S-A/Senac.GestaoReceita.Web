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

        //✔
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

        //✔
        public ActionResult AdicionarEstado(EstadoViewModel novoEstado)
        {
            List<EstadoViewModel> minhaLista = getEstados();

            if (ModelState.IsValid)
            {
                bool estadoJaExiste = minhaLista.Any(p => p.descricaoEstado.Equals(novoEstado.descricaoEstado, StringComparison.OrdinalIgnoreCase));

                if (estadoJaExiste)
                {
                    ModelState.AddModelError("estadoExistente", "Este estado já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });                    
                }

                List<PaisViewModel> listaPaises = this.getPaises();

                novoEstado.idPais = 0;

                foreach (var meuPais in listaPaises)
                {
                    if (meuPais.descricaoPais == novoEstado.descricaoPais)
                    {
                        novoEstado.idPais = meuPais.id;
                    }
                }

                int novoId = minhaLista.Max(max => max.id) + 1;

                this.CadastrarNovoEstado(novoEstado, novoId);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("estadoExistente", "Este estado já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
            }

            return PartialView("_CreateModalPartial", minhaLista);
        }

        private void CadastrarNovoEstado(EstadoViewModel novoEstado, int novoId)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { idPais = novoEstado.idPais, descricaoEstado = novoEstado.descricaoEstado, id = novoId }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Estados", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {                    
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<EstadoTO>(stringResult.Result);
                }
                else
                {                    
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }
        }

        //✔
        public ActionResult EditarEstado(EstadoViewModel estadoEditar)
        {
            var paises = getPaises();

            EstadoViewModel dadosEstado = getEstadoById(estadoEditar.id);

            EstadoViewModel estadoView = new EstadoViewModel()
            {
                id = dadosEstado.id,
                descricaoEstado = dadosEstado.descricaoEstado,
                idPais = dadosEstado.idPais,
                pais = dadosEstado.pais,
                listaPaises = paises
            };

            List<EstadoViewModel> minhaLista = getEstados();

            if (this.ModelState.IsValid)
            {
                bool estadoJaExiste = minhaLista.Any(p => p.descricaoEstado.Equals(estadoEditar.descricaoEstado, StringComparison.OrdinalIgnoreCase));

                if (estadoJaExiste)
                {
                    ModelState.AddModelError("estadoExistente", "Este estado já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }

                this.NovaEdicaoEstado(estadoEditar, dadosEstado);

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("estadoExistente", "Este estado já existe.");
                    var erros = ModelState.Values.SelectMany(v => v.Errors).ToList();

                    return Json(new { success = false, erros });
                }
            }

            return PartialView("_UpdateModalPartial", estadoView);
        }

        private void NovaEdicaoEstado(EstadoViewModel estadoEditar, EstadoViewModel dadosEstado)
        {
            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { idPais = estadoEditar.idPais, descricaoEstado = estadoEditar.descricaoEstado, id = estadoEditar.id }), Encoding.UTF8, "application/json");

                var response = client.PutAsync(new Uri("http://gestaoreceitaapi.somee.com/api/Estados/" + dadosEstado.id), formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {                    
                    var stringResult = response.Result.Content.ReadAsStringAsync();
                    var objectJson = JsonConvert.DeserializeObject<EstadoTO>(stringResult.Result);
                }
                else
                {                    
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);
                }
            }
        }


        //Lorenzo disse que Nairo comentou sobre ser problema na API.
        public ActionResult DeletarEstado(PaisViewModel paisDeletar)
        {
            return Json(new { });
        }

        //Partial Create
        public ActionResult getModalPaises()
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Pais");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var paisToList = JsonConvert.DeserializeObject<List<PaisTO>>(stringResult.Result);
                    List<PaisViewModel> paisViewModelList = new List<PaisViewModel>();

                    foreach (var paisTo in paisToList)
                    {
                        PaisViewModel paisvm = new PaisViewModel
                        {
                            id = paisTo.id,
                            descricaoPais = paisTo.descricaoPais,
                        };

                        paisViewModelList.Add(paisvm);
                    }

                    return PartialView("_CreateModalPartial", paisViewModelList);
                }
                else
                {
                    //mensagem de erro de validação
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);

                }
            }

            return Index();
        }

        //Partial Update
        public ActionResult getPaisesAndById(EstadoViewModel estadoViewModel)
        {
            var paises = getPaises();

            EstadoViewModel EstadosDados = getEstadoById(estadoViewModel.id);

            EstadoViewModel EstadoView = new EstadoViewModel()
            {
                id = EstadosDados.id,
                descricaoEstado = EstadosDados.descricaoEstado,
                idPais = EstadosDados.idPais,
                pais = EstadosDados.pais,
                listaPaises = paises
            };

            return PartialView("_UpdateModalPartial", EstadoView);

        }


        //CONSULTAS:
        private List<PaisViewModel> getPaises()
        {

            List<PaisViewModel> listPaisViewModel = new List<PaisViewModel>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Pais");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var paisToList = JsonConvert.DeserializeObject<List<PaisTO>>(stringResult.Result);
                    List<PaisViewModel> paisViewModelList = new List<PaisViewModel>();

                    foreach (var paisTo in paisToList)
                    {
                        PaisViewModel paisvm = new PaisViewModel
                        {
                            id = paisTo.id,
                            descricaoPais = paisTo.descricaoPais,
                        };

                        paisViewModelList.Add(paisvm);
                    }

                    return listPaisViewModel = paisViewModelList;
                }
                else
                {
                    //mensagem de erro de validação
                    var content = response.Result.Content.ReadAsStringAsync();
                    var ret = JsonConvert.DeserializeObject<ValidationResult>(content.Result);

                }
            }

            return listPaisViewModel;
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

    }
}