﻿namespace GestaoClientesEBeneficiarios.Domain.Entidades
{
    public class Beneficiario
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public long IdCliente { get; set; }
    }
}
