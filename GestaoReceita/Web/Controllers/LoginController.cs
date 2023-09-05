using Newtonsoft.Json;
using SolutionWebCadastroLogin.Models;
using SolutionWebCadastroLogin.Models.Usuarios;
using SolutionWebCadastroLogin.Models.UsuariosCadastro;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Models.Usuario;

namespace SolutionWebCadastroLogin.Controllers
{
    public class LoginController : LoginExtention
    {
        public ActionResult Index(string mensagemErro, string mensagemSucesso)
        {
            LoginUsuario loginUsuario = new LoginUsuario();

            loginUsuario.mensagemErroAutenticacao = mensagemErro;
            loginUsuario.mensagemSucesso = mensagemSucesso;

            return View(loginUsuario);
        }

        public ActionResult Login(string Username, string Password)
        {
            var mensagemErro = string.Empty;

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(new { Username = Username, senha = Password }), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/Login", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<LoginUsuarioRequest>(stringResult.Result);

                    saveUsuarioSessao("Username", objectJson.Id);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    mensagemErro = response.Result.ReasonPhrase;
                }
            }
            return RedirectToAction("Index", new { mensagemErro = mensagemErro });
        }

        public ActionResult Editar()
        {
            var id = Session["IdUsuario"] as int?;

            var usuario = GetUsuarioById(id.GetValueOrDefault());
           
            return RedirectToAction("Cadastro", usuario);
        }

        public ActionResult Logout()
        {
            limparUsuarioSessao();

            return RedirectToAction("Index");
        }

        private DadosUsuarioViewModel GetUsuarioById(int id)
        {
            var usuario = new DadosUsuarioViewModel();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/"+id);


                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    usuario = JsonConvert.DeserializeObject<DadosUsuarioViewModel>(stringResult.Result);
                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var erro = response.Result.ReasonPhrase;
                }
            }
            return usuario;
        }

        private List<DadosUsuarioViewModel> GetListaUsuarios()
        {
            var listaUsuarios = new List<DadosUsuarioViewModel>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/");


                response.Wait();
                if(response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    listaUsuarios = JsonConvert.DeserializeObject<List<DadosUsuarioViewModel>>(stringResult.Result);
                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var erro = response.Result.ReasonPhrase;
                }
            }
            return listaUsuarios;
        }

        public ActionResult Cadastro(DadosUsuarioViewModel cadastro)
        {
            var listaEmpresas = getListaEmpresas();

            DadosUsuarioViewModel dadosViewModel = cadastro != null ? cadastro : new DadosUsuarioViewModel();

            dadosViewModel.listaEmpresas = listaEmpresas;

            return View(dadosViewModel);
        }





        public ActionResult Usuario()
        {
            var listaUsuarios = GetListaUsuarios();

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel.ListaUsuarios = listaUsuarios;

            return View(usuarioViewModel);

        }






        private List<Empresa> getListaEmpresas()
        {
            var listaEmpresas = new List<Empresa>();

            using (var client = new HttpClient())
            {
                var response = client.GetAsync("http://gestaoreceitaapi.somee.com/api/Empresas");

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    listaEmpresas = JsonConvert.DeserializeObject<List<Empresa>>(stringResult.Result);

                }
                else
                {
                    //Erro de requisicao
                    var content = response.Result.Content.ReadAsStringAsync();

                    var ret = JsonConvert.DeserializeObject(content.Result);
                }
            }
            return listaEmpresas;
        }

        public ActionResult PersistirCadastro(int? Id, string Nome, string Usuario, string Senha, int Empresa)
        {
            var mensagemErro = "";

            if (Id != null && Id > 0)
            {
                mensagemErro = updateUsuario(Id.GetValueOrDefault(), Nome, Usuario, Senha, Empresa);
            }
            else
            {
                mensagemErro = cadastrarUsuario(Nome, Usuario, Senha, Empresa);
            }

            if (!string.IsNullOrEmpty(mensagemErro))
            {
                return RedirectToAction("Cadastro", new DadosUsuarioViewModel()
                {
                    id = Id.GetValueOrDefault(),
                    nome = Nome,
                    acesso = 0,
                    ativo = 1,
                    empresaId = Empresa,
                    manterLogado = 0,
                    username = Usuario,
                    senha = Senha,
                    mensagemErro = mensagemErro,
                    mensagemSucesso = "Dados salvos com sucesso",
                });
            }

            return RedirectToAction("Cadastro");
        }

        public string updateUsuario(int id, string Nome, string Usuario, string Senha, int Empresa)
        {
            var mensagemErro = "";

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        id = id,
                        nome = Nome,
                        username = Usuario,
                        empresaId = Empresa,
                        senha = Senha,
                        acesso = 0,
                        manterLogado = 0,
                        ativo = 1
                    }
                ), Encoding.UTF8, "application/json");

                var response = client.PutAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/"+id, formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<CadastroUsuarioRequest>(stringResult.Result);

                    return string.Empty;
                }
                else
                {
                    mensagemErro = response.Result.ReasonPhrase;
                }
            }

            return mensagemErro;
        }


        public string cadastrarUsuario(string Nome, string Usuario, string Senha, int Empresa)
        {
            var mensagemErro = "";

            using (var client = new HttpClient())
            {
                var formContentString = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        nome = Nome,
                        username = Usuario,
                        empresaId = Empresa,
                        senha = Senha,
                        acesso = 0,
                        manterLogado = 0,
                        ativo = 1
                    }
                ), Encoding.UTF8, "application/json");

                var response = client.PostAsync("http://gestaoreceitaapi.somee.com/api/Usuarios", formContentString);

                response.Wait();

                if (response.Result.IsSuccessStatusCode)
                {
                    var stringResult = response.Result.Content.ReadAsStringAsync();

                    var objectJson = JsonConvert.DeserializeObject<CadastroUsuarioRequest>(stringResult.Result);

                    return string.Empty;
                }
                else
                {
                    mensagemErro = response.Result.ReasonPhrase;
                }
            }

            return mensagemErro;
        }

        public JsonResult DeletarUsuario(int id)
        {
            var msg = "Deletado com sucesso";
            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync("http://gestaoreceitaapi.somee.com/api/Usuarios/" + id);
                response.Wait();

                if (!response.Result.IsSuccessStatusCode)
                {
                    msg = response.Result.ReasonPhrase;

                }

            }
            return Json(new { msgRetorno = msg });
        }

    }
}







