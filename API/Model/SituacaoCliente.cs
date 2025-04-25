namespace API.Model
{
    public class SituacaoCliente
    {
        public int Id { get; set; }

        public string Descricao { get; set; } = null!;

        public virtual ICollection<Cliente> Clientes { get; } = new List<Cliente>();

    }
}
