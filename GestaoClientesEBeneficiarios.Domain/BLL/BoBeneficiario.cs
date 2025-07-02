using GestaoClientesEBeneficiarios.Domain.DAL;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using System;
using System.Collections.Generic;

namespace GestaoClientesEBeneficiarios.Domain.BLL
{
    public class BoBeneficiario
    {
        private readonly DaoBeneficiario _daoBeneficiario;
        public BoBeneficiario(DaoBeneficiario daoBeneficiario)
        {
            _daoBeneficiario = daoBeneficiario;
        }
        public long Incluir(Beneficiario beneficiario)
        {
            ValidarCpfCliente(beneficiario);
            return _daoBeneficiario.Incluir(beneficiario);
        }

        public void Alterar(Beneficiario beneficiario)
        {
            ValidarCpfCliente(beneficiario);
            _daoBeneficiario.Alterar(beneficiario);
        }

        public void Excluir(long id)
        {
            _daoBeneficiario.Excluir(id);
        }

        public List<Beneficiario> Listar(long idCliente)
        {
            return _daoBeneficiario.Consultar(idCliente);
        }

        public bool VerificarExistencia(string CPF, long? id = null)
        {
            return _daoBeneficiario.VerificarExistencia(CPF, id);
        }

        private void ValidarCpfCliente(Beneficiario beneficiario)
        {
            if (!BoValidacaoCpf.Validar(beneficiario.CPF))
                throw new InvalidOperationException("CPF inválido. Por favor, verifique e informe um CPF válido.");

            if (VerificarExistencia(beneficiario.CPF, beneficiario.Id))
                throw new InvalidOperationException("Já existe um cliente com este CPF informado.");
        }
    }
}
