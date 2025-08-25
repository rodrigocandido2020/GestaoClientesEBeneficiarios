using GestaoClientesEBeneficiarios.Domain.Entidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GestaoClientesEBeneficiarios.Domain.DAL
{
    public class DaoBeneficiario : AcessoDados
    {
        private const int TabelaPrincipal = 0;
        private const int LinhaPrincipal = 0;
        private const int ColunaId = 0;
        private const int QuantidadeMinimaDeLinhas = 1;

        internal long Incluir(Beneficiario beneficiario)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IdCliente", beneficiario.IdCliente)
            };

            var dataSet = Consultar("SP_IncBeneficiario", parametros);

            long idNulo = 0;
            if (dataSet.Tables[TabelaPrincipal].Rows.Count >= QuantidadeMinimaDeLinhas)
                long.TryParse(dataSet.Tables[TabelaPrincipal].Rows[LinhaPrincipal][ColunaId].ToString(), out idNulo);
            return idNulo;
        }

        internal List<Beneficiario> Consultar(long idCliente)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdCliente", idCliente)
            };

            var dataSet = Consultar("SP_ListarBeneficiarios", parametros);

            var beneficiarios = Converter(dataSet);

            return beneficiarios;
        }

        internal void Alterar(Beneficiario beneficiario)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", beneficiario.Id),
                new SqlParameter("Nome", beneficiario.Nome),
                new SqlParameter("CPF", beneficiario.CPF),
                new SqlParameter("IdCliente", beneficiario.IdCliente)
            };

            Executar("SP_AltBeneficiario", parametros);
        }

        internal void Excluir(long id)
        {
            var parametros = new List<SqlParameter>
            {
                new SqlParameter("Id", id)
            };

            Executar("SP_DelBeneficiario", parametros);
        }

        internal bool VerificarExistencia(string CPF, long? id)
        {
            List<SqlParameter> parametros = new List<SqlParameter> 
            {
                new SqlParameter("CPF", CPF),
                new SqlParameter("Id", id)
            };

            var dataSet = Consultar("SP_VerificaBeneficiarios", parametros);

            return dataSet.Tables[TabelaPrincipal].Rows.Count >= QuantidadeMinimaDeLinhas;
        }

        private List<Beneficiario> Converter(DataSet dataSet)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count >= QuantidadeMinimaDeLinhas && dataSet.Tables[TabelaPrincipal].Rows.Count >= QuantidadeMinimaDeLinhas)
            {
                foreach (DataRow row in dataSet.Tables[TabelaPrincipal].Rows)
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
