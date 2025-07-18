using GestaoClientesEBeneficiarios.Domain.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GestaoClientesEBeneficiarios.Domain.DAL
{
    public class DaoCliente : AcessoDados
    {
        private const int TabelaPrincipal = 0;
        private const int LinhaPrincipal = 0;
        private const int ColunaId = 0;
        private const int QuantidadeMinimaDeLinhas = 1;

        internal long Incluir(Cliente cliente)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", cliente.Nome),
                new SqlParameter("Sobrenome", cliente.Sobrenome),
                new SqlParameter("Nacionalidade", cliente.Nacionalidade),
                new SqlParameter("CEP", cliente.CEP),
                new SqlParameter("Estado", cliente.Estado),
                new SqlParameter("Cidade", cliente.Cidade),
                new SqlParameter("Logradouro", cliente.Logradouro),
                new SqlParameter("Email", cliente.Email),
                new SqlParameter("Telefone", cliente.Telefone),
                new SqlParameter("CPF", cliente.CPF)
            };

            var dataSet = Consultar("SP_IncCliente", parametros);
            long idNulo = 0;
            if (dataSet.Tables[TabelaPrincipal].Rows.Count >= QuantidadeMinimaDeLinhas)
                long.TryParse(dataSet.Tables[TabelaPrincipal].Rows[LinhaPrincipal][ColunaId].ToString(), out idNulo);
            return idNulo;
        }

        internal Cliente Consultar(long Id)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", Id)
            };

            var dataSet = Consultar("SP_ConsCliente", parametros);

            var clientes = Converter(dataSet);

            return clientes.FirstOrDefault();
        }

        internal bool VerificarExistencia(string CPF, long? id)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("CPF", CPF),
                new SqlParameter("Id", id)
            };

            var dataSet = Consultar("SP_VerificaCliente", parametros);

            return dataSet.Tables[TabelaPrincipal].Rows.Count >= QuantidadeMinimaDeLinhas;
        }

        internal List<Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("iniciarEm", iniciarEm),
                new SqlParameter("quantidade", quantidade),
                new SqlParameter("campoOrdenacao", campoOrdenacao),
                new SqlParameter("crescente", crescente)
            };

            var dataSet = Consultar("SP_PesqCliente", parametros);

            var clientes = Converter(dataSet);

            int iQtd = 0;

            if (dataSet.Tables.Count > 1 && dataSet.Tables[1].Rows.Count > 0)
                int.TryParse(dataSet.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return clientes;
        }

        internal List<Cliente> Listar()
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Id", 0));

            DataSet ds = base.Consultar("SP_ConsCliente", parametros);
            List<Cliente> cli = Converter(ds);

            return cli;
        }

        internal void Alterar(Cliente cliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Nome", cliente.Nome));
            parametros.Add(new SqlParameter("Sobrenome", cliente.Sobrenome));
            parametros.Add(new SqlParameter("Nacionalidade", cliente.Nacionalidade));
            parametros.Add(new SqlParameter("CEP", cliente.CEP));
            parametros.Add(new SqlParameter("Estado", cliente.Estado));
            parametros.Add(new SqlParameter("Cidade", cliente.Cidade));
            parametros.Add(new SqlParameter("Logradouro", cliente.Logradouro));
            parametros.Add(new SqlParameter("Email", cliente.Email));
            parametros.Add(new SqlParameter("Telefone", cliente.Telefone));
            parametros.Add(new SqlParameter("CPF", cliente.CPF));
            parametros.Add(new SqlParameter("ID", cliente.Id));

            base.Executar("SP_AltCliente", parametros);
        }

        internal void Excluir(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Id", Id));

            base.Executar("SP_DelCliente", parametros);
        }

        private List<Cliente> Converter(DataSet ds)
        {
            List<Cliente> lista = new List<Cliente>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Cliente cli = new Cliente();
                    cli.Id = row.Field<long>("Id");
                    cli.CEP = row.Field<string>("CEP");
                    cli.Cidade = row.Field<string>("Cidade");
                    cli.Email = row.Field<string>("Email");
                    cli.Estado = row.Field<string>("Estado");
                    cli.Logradouro = row.Field<string>("Logradouro");
                    cli.Nacionalidade = row.Field<string>("Nacionalidade");
                    cli.Nome = row.Field<string>("Nome");
                    cli.Sobrenome = row.Field<string>("Sobrenome");
                    cli.Telefone = row.Field<string>("Telefone");
                    cli.CPF = row.Field<string>("CPF");
                    lista.Add(cli);
                }
            }

            return lista;
        }
    }
}
