  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GestaoClientesEBeneficiarios.Domain.BLL;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using GestaoClientesEBeneficiarios.Web.ViewModels;

namespace GestaoClientesEBeneficiarios.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly BoCliente _boCliente;
        private readonly IMapper _mapper;

        public ClienteController(BoCliente boCliente, IMapper mapper)
        {
            _boCliente = boCliente;
            _mapper = mapper;

        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaClientes(int deslocamento = 0, int tamanhoDaPagina = 0, string odernacao = null)
        {
            var quantidade = 0;
            var campo = string.Empty;
            var crescente = string.Empty;
            string[] array = odernacao.Split(' ');

            if (array.Length > 0)
                campo = array[0];

            if (array.Length > 1)
                crescente = array[1];

            var clientes = _boCliente.Pesquisa(deslocamento, tamanhoDaPagina, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out quantidade);


            var clientesViewModel = _mapper.Map<IEnumerable<ClienteViewModel>>(clientes);

            return Json(new
            {
                Result = "OK",
                Records = clientesViewModel,
                TotalRecordCount = quantidade
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            try
            {
                var cliente = new Cliente
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                };

                model.Id = _boCliente.Incluir(cliente);

                return Json("Cadastro efetuado com sucesso");
            }
            catch (InvalidOperationException ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("Erro inesperado: " + ex.Message);
            }
        }


        [HttpPost]
        public JsonResult Alterar(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var erros = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }

            try
            {
                var cliente = new Cliente
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                };

                _boCliente.Alterar(cliente);

                return Json("Cadastro alterado com sucesso");
            }
            catch (InvalidOperationException ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("Erro inesperado: " + ex.Message);
            }
        }


        [HttpGet]
        public ActionResult Alterar(long id)
        {
            var cliente = _boCliente.Consultar(id);

            var model = cliente is null ? null : new ClienteViewModel
            {
                Id = cliente.Id,
                CEP = cliente.CEP,
                Cidade = cliente.Cidade,
                Email = cliente.Email,
                Estado = cliente.Estado,
                Logradouro = cliente.Logradouro,
                Nacionalidade = cliente.Nacionalidade,
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                Telefone = cliente.Telefone,
                CPF = cliente.CPF,
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult Excluir(long id)
        {
            try
            {
                _boCliente.Excluir(id);

                return Json(new { Result = "OK", Message = "Cadastro excluído com sucesso" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { Result = "ERROR", Message = "Erro ao excluir cliente: " + ex.Message });
            }
        }



        //[HttpPost]
        //public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        //{
        //    try
        //    {
        //        int qtd = 0;
        //        string campo = string.Empty;
        //        string crescente = string.Empty;
        //        string[] array = jtSorting.Split(' ');

        //        if (array.Length > 0)
        //            campo = array[0];

        //        if (array.Length > 1)
        //            crescente = array[1];

        //        var clientes = _boCliente.Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

        //        //Return result to jTable
        //        return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}


    }
}