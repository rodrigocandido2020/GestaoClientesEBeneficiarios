﻿namespace GestaoClientesEBeneficiarios.Domain.Entidades
{
    public class Cliente
    {
        public long Id { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public string Logradouro { get; set; }
        public string Nacionalidade { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
    }    
}
