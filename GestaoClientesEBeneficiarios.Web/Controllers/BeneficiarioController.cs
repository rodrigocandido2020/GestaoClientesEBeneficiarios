using System;
using System.Web.Mvc;
using GestaoClientesEBeneficiarios.Domain.BLL;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using GestaoClientesEBeneficiarios.Web.Models;

namespace GestaoClientesEBeneficiarios.Web.Controllers
{
    public class BeneficiarioController : Controller
    {
        [HttpPost]
        public JsonResult Incluir(BeneficiarioModel model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                return Json("Dados inválidos");
            }

            BoBeneficiario bo = new BoBeneficiario();
            var beneficiario = new Beneficiario
            {
                Nome = model.Nome,
                CPF = model.CPF,
                IdCliente = model.IdCliente
            };

            try
            {
                model.Id = bo.Incluir(beneficiario);
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
                BoBeneficiario bo = new BoBeneficiario();
                var beneficiarios = bo.Listar(idCliente);

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

            BoBeneficiario bo = new BoBeneficiario();
            bo.Alterar(new Beneficiario
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
            BoBeneficiario bo = new BoBeneficiario();
            bo.Excluir(id);
            return Json("Beneficiário excluído com sucesso");
        }
    }
}
