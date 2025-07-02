using GestaoClientesEBeneficiarios.Domain.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GestaoClientesEBeneficiarios.Domain.DAL
{
    internal class DaoCliente : AcessoDados
    {
        internal long Incluir(Cliente cliente)
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

            DataSet ds = base.Consultar("SP_IncCliente", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal Cliente Consultar(long Id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Id", Id));

            DataSet ds = base.Consultar("SP_ConsCliente", parametros);
            List<Cliente> cli = Converter(ds);

            return cli.FirstOrDefault();
        }

        internal bool VerificarExistencia(string CPF, long? id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("CPF", CPF));
            parametros.Add(new SqlParameter("Id", id));

            DataSet ds = base.Consultar("SP_VerificaCliente", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal List<Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("iniciarEm", iniciarEm));
            parametros.Add(new SqlParameter("quantidade", quantidade));
            parametros.Add(new SqlParameter("campoOrdenacao", campoOrdenacao));
            parametros.Add(new SqlParameter("crescente", crescente));

            DataSet ds = base.Consultar("SP_PesqCliente", parametros);
            List<Cliente> cli = Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return cli;
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
