using System.ComponentModel.DataAnnotations;

namespace dot_net_junior.Models
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Digite o nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o nome")]
        public string CPF_CNPJ { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de documento")]
        public string TipoDocumento { get; set; }
    }
}
