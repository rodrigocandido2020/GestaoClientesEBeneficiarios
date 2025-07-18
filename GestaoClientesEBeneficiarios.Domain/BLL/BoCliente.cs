using GestaoClientesEBeneficiarios.Domain.DAL;
using GestaoClientesEBeneficiarios.Domain.Entidades;
using System;
using System.Collections.Generic;

namespace GestaoClientesEBeneficiarios.Domain.BLL
{
    public class BoCliente
    {
        private readonly DaoCliente _daoCliente;

        public BoCliente(DaoCliente daoCliente)
        {
            _daoCliente = daoCliente;
        }
        public long Incluir(Cliente cliente)
        {
            ValidarCpfCliente(cliente);
            return _daoCliente.Incluir(cliente);
        }

        public void Alterar(Cliente cliente)
        {
            ValidarCpfCliente(cliente);
            _daoCliente.Alterar(cliente);
        }

        public Cliente Consultar(long id)
        {
            return _daoCliente.Consultar(id);
        }

        public void Excluir(long id)
        {
            _daoCliente.Excluir(id);
        }

        public List<Cliente> Listar()
        {
            return _daoCliente.Listar();
        }

        public List<Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            return _daoCliente.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        public bool VerificarExistencia(string CPF, long? id = null)
        {
            return _daoCliente.VerificarExistencia(CPF, id);
        }

        private void ValidarCpfCliente(Cliente cliente)
        {
            if (!BoValidacaoCpf.Validar(cliente.CPF))
                throw new InvalidOperationException("CPF inválido. Por favor, verifique e informe um CPF válido.");

            if (VerificarExistencia(cliente.CPF, cliente.Id))
                throw new InvalidOperationException("Já existe um cliente com este CPF informado.");
        }
    }
}
 