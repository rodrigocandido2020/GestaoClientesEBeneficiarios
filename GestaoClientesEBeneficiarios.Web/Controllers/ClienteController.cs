  using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GestaoClientesEBeneficiarios.Domain.BLL;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using GestaoClientesEBeneficiarios.Web.Models;

namespace GestaoClientesEBeneficiarios.Web.Controllers
{
    public class ClienteController : Controller
    {
        private const int INDICE_CAMPO_ORDENACAO = 0;
        private const int INDICE_DIRECAO_ORDENACAO = 1;

        private const int INDICE_INICIO_PAGINA_PADRAO = 0;
        private const int TAMANHO_PAGINA_PADRAO = 10;

        private const string ORDENACAO_PADRAO = "Nome ASC";
        private const string CAMPO_ORDENACAO_PADRAO = "Nome";
        private const string DIRECAO_CRESCENTE = "ASC";

        private readonly BoCliente _boCliente;
        private readonly IMapper _mapper;
        public ClienteController(BoCliente boCliente, IMapper mapper)
        {
            _boCliente = boCliente;
            _mapper = mapper;

        }

        [Route("lista-de-clientes")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("lista-clientes-json")]
        [HttpGet]
        public JsonResult ListaClientes(
            int indiceInicioPagina = INDICE_INICIO_PAGINA_PADRAO,
            int tamanhoPagina = TAMANHO_PAGINA_PADRAO,
            string ordenacao = ORDENACAO_PADRAO)
        {
            int totalRegistros;

            string[] partesOrdenacao = (ordenacao ?? "").Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);

            string campoOrdenacao = partesOrdenacao.Length > INDICE_CAMPO_ORDENACAO
                ? partesOrdenacao[INDICE_CAMPO_ORDENACAO]
                : CAMPO_ORDENACAO_PADRAO;

            bool crescente = partesOrdenacao.Length > INDICE_DIRECAO_ORDENACAO
                && partesOrdenacao[INDICE_DIRECAO_ORDENACAO].Equals(DIRECAO_CRESCENTE, StringComparison.InvariantCultureIgnoreCase);

            var clientes = _boCliente.Pesquisa(indiceInicioPagina, tamanhoPagina, campoOrdenacao, crescente, out totalRegistros);

            var clientesViewModel = _mapper.Map<IEnumerable<ClienteViewModel>>(clientes);

            return Json(new
            {
                Result = "OK",
                Records = clientesViewModel,
                TotalRecordCount = totalRegistros
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



        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                var clientes = _boCliente.Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


    }
}