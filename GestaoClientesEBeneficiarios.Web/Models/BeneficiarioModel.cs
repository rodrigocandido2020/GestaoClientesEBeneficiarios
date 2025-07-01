using System.ComponentModel.DataAnnotations;

namespace GestaoClientesEBeneficiarios.Web.Models 
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "CPF")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Informe um CPF válido no formato 000.000.000-00")]
        public string CPF { get; set; }

        public long IdCliente { get; set; }
    }
}