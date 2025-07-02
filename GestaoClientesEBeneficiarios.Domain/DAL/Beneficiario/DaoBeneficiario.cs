using GestaoClientesEBeneficiarios.Domain.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GestaoClientesEBeneficiarios.Domain.DAL
{
    public class DaoBeneficiario : AcessoDados
    {
        public long Incluir(Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();


            parametros.Add(new SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new SqlParameter("IdCliente", beneficiario.IdCliente));

            DataSet ds = base.Consultar("SP_IncBeneficiario", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        public List<Beneficiario> Consultar(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@IdCliente", idCliente));

            DataSet ds = Consultar("SP_ListarBeneficiarios", parametros);

            List<Beneficiario> cli = Converter(ds);

            return cli;
        }


        public void Alterar(Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", beneficiario.Id),
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IdCliente", beneficiario.IdCliente)
            };

            Executar("SP_AltBeneficiario", parametros);
        }

        public void Excluir(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", id)
            };

            Executar("SP_DelBeneficiario", parametros);
        }

        public bool VerificarExistencia(string CPF, long? id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("CPF", CPF));
            parametros.Add(new SqlParameter("Id", id));

            DataSet ds = base.Consultar("SP_VerificaBeneficiarios", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario ben = new Beneficiario();
                    ben.Id = row.Field<long>("Id");
                    ben.Nome = row.Field<string>("Nome");
                    ben.CPF = row.Field<string>("CPF");
                    ben.IdCliente = row.Field<long>("IdCliente");
                    lista.Add(ben);
                }
            }

            return lista;
        }
    }
}
