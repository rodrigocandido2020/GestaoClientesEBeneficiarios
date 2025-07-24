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
        const int VALOR_PADRAO = 0;
        const int QUANTIDADE_MINIMA = 1;
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
        public JsonResult ListaClientes(int jtStartIndex = VALOR_PADRAO, int jtPageSize = VALOR_PADRAO, string jtSorting = null)
        {
            var quantidadeTotalUsuario = VALOR_PADRAO;
            var campo = string.Empty;
            var crescente = string.Empty;
            string[] array = jtSorting.Split(' ');

            if (array.Length > VALOR_PADRAO)
                campo = array[VALOR_PADRAO];

            if (array.Length > QUANTIDADE_MINIMA)
                crescente = array[QUANTIDADE_MINIMA];

            var clientes = _boCliente.Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out quantidadeTotalUsuario);


            var clientesViewModel = _mapper.Map<IEnumerable<ClienteViewModel>>(clientes);

            return Json(new
            {
                Result = "OK",
                Records = clientesViewModel,
                TotalRecordCount = quantidadeTotalUsuario
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
    }
}