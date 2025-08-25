using AutoMapper;
using GestaoClientesEBeneficiarios.Domain.BLL;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using GestaoClientesEBeneficiarios.Web.ViewModels;
using System;
using System.Web.Mvc;

namespace GestaoClientesEBeneficiarios.Web.Controllers
{
    public class BeneficiarioController : Controller
    {
        private readonly BoBeneficiario _boBeneficiario;
        private readonly IMapper _mapper;

        public BeneficiarioController(BoBeneficiario boBeneficiario, IMapper mapper)
        {
            _boBeneficiario = boBeneficiario;
            _mapper = mapper;
        }

        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel beneficiarioModel)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json("Dados inválidos");
            }

            try
            {
                var beneficiario = _mapper.Map<Beneficiario>(beneficiarioModel);
                beneficiarioModel.Id = _boBeneficiario.Incluir(beneficiario);
                return Json("Beneficiário incluído com sucesso!");
            }
            catch (InvalidOperationException ex)
            {
                Response.StatusCode = 400;
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Listar(long idCliente)
        {
            try
            {
                var beneficiarios = _boBeneficiario.Listar(idCliente);

                return Json(new { Result = "OK", Records = beneficiarios });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Alterar(BeneficiarioModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json("Dados inválidos");
            }

            _boBeneficiario.Alterar(new Beneficiario
            {
                Id = model.Id,
                Nome = model.Nome,
                CPF = model.CPF,
                IdCliente = model.IdCliente
            });

            return Json("Beneficiário alterado com sucesso");
        }

        [HttpPost]
        public JsonResult Excluir(long id)
        {
            _boBeneficiario.Excluir(id);
            return Json("Beneficiário excluído com sucesso");
        }
    }
}
