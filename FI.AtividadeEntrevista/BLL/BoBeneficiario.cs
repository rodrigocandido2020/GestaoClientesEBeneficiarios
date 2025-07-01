using GestaoClientesEBeneficiarios.Domain.DAL;
using GestaoClientesEBeneficiarios.Domain.DML;
using System;
using System.Collections.Generic;

namespace GestaoClientesEBeneficiarios.Domain.BLL
{
    public class BoBeneficiario
    {
        public long Incluir(Beneficiario beneficiario)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            ValidarCpfCliente(beneficiario);
            return ben.Incluir(beneficiario);
        }

        public void Alterar(Beneficiario beneficiario)
        {
            DaoBeneficiario ben = new DaoBeneficiario();
            ValidarCpfCliente(beneficiario);
            ben.Alterar(beneficiario);
        }

        public void Excluir(long id)
        {
            DAL.DaoBeneficiario ben = new DaoBeneficiario();
            ben.Excluir(id);
        }

        public List<Beneficiario> Listar(long idCliente)
        {
            DaoBeneficiario dao = new DaoBeneficiario();
            return dao.Consultar(idCliente);
        }



        public bool VerificarExistencia(string CPF, long? id = null)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.VerificarExistencia(CPF, id);
        }

        private void ValidarCpfCliente(DML.Beneficiario beneficiario)
        {
            if (!BoValidacaoCpf.Validar(beneficiario.CPF))
                throw new InvalidOperationException("CPF inválido. Por favor, verifique e informe um CPF válido.");

            if (VerificarExistencia(beneficiario.CPF, beneficiario.Id))
                throw new InvalidOperationException("Já existe um cliente com este CPF informado.");
        }
    }
}
