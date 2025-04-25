using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Cliente
    {
        public int Id { get; set; }  
        [Required]
        public string Nome { get; set; } = null!;

        [Required]
        [StringLength(11)] 
        public string Cpf { get; set; } = null!;

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [StringLength(1)] 
        public string Sexo { get; set; } = null!;

        [Required]
        public int SituacaoClienteId { get; set; }

        public virtual SituacaoCliente? SituacaoCliente { get; set; }



    }
}
