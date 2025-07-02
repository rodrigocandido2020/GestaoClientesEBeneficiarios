using System.ComponentModel.DataAnnotations;

namespace GestaoClientesEBeneficiarios.Web.Models
{
    public class ClienteModel
    {
        public long Id { get; set; }

        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "Digite um CEP válido no formato 00000-000")]
        [Required]
        public string CEP { get; set; }

        [Required]
        public string Cidade { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Digite um e-mail válido")]
        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        [Required]
        public string Logradouro { get; set; }

        [Required]
        public string Nacionalidade { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sobrenome { get; set; }

        public string Telefone { get; set; }

        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$", ErrorMessage = "Digite um CPF válido no formato 000.000.000-00")]
        [Required]
        public string CPF { get; set; }

    }    
}