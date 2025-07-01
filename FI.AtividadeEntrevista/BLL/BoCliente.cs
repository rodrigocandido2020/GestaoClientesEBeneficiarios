using GestaoClientesEBeneficiarios.Domain.DAL;
using GestaoClientesEBeneficiarios.Domain.DML;
using System;
using System.Collections.Generic;

namespace GestaoClientesEBeneficiarios.Domain.BLL
{
    public class BoCliente
    {
        public long Incluir(Cliente cliente)
        {
            DaoCliente cli = new DaoCliente();
            ValidarCpfCliente(cliente);
            return cli.Incluir(cliente);
        }

        public void Alterar(Cliente cliente)
        {
            DaoCliente cli = new DaoCliente();
            ValidarCpfCliente(cliente);
            cli.Alterar(cliente);
        }

        public Cliente Consultar(long id)
        {
            DaoCliente cli = new DaoCliente();
            return cli.Consultar(id);
        }

        public void Excluir(long id)
        {
            DaoCliente cli = new DaoCliente();
            cli.Excluir(id);
        }

        public List<Cliente> Listar()
        {
            DaoCliente cli = new DaoCliente();
            return cli.Listar();
        }

        public List<Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DaoCliente cli = new DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        public bool VerificarExistencia(string CPF, long? id = null)
        {
            DaoCliente cli = new DaoCliente();
            return cli.VerificarExistencia(CPF, id);
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
 