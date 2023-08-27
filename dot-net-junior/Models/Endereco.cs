using System.ComponentModel.DataAnnotations;

namespace dot_net_junior.Models
{
    public class Endereco
    {
            [Key]
            public int ID { get; set; }

            [Required(ErrorMessage = "Digite a rua")]
            public string Rua { get; set; }

            [Required(ErrorMessage = "Digite o numero")]
            public string Numero { get; set; }

            [Required(ErrorMessage = "Digite o CEP")]
            public string CEP { get; set; }

            [Required(ErrorMessage = "Digite o bairro")]
            public string Bairro { get; set; }

            [Required(ErrorMessage = "Digite a cidade")]
            public string Cidade { get; set; }

            [Required(ErrorMessage = "Digite informe o tipo de endereço")]
            public string TipoEndereco { get; set; }
            public int IDcliente { get; set; }
        
    }
}
