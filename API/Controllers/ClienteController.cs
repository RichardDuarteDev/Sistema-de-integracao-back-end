using API.DAL;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly Contexto _context;

        public ClienteController(Contexto context)
        {
            _context = context;
        }

        

        [HttpGet(Name = "Clientes")]
        public IEnumerable<Cliente> Get()
        {
            return _context.ConsultarClientes();
        }

        [HttpGet("{id}", Name = "GetCliente")]
        public IActionResult Get(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado");
            }
            return Ok(cliente);
        }



        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null)
                {
                    return BadRequest("Cliente não pode ser nulo.");
                }

                // Verifica se o CPF já está em uso
                var existingCliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Cpf == cliente.Cpf);

                if (existingCliente != null)
                {
                    return Conflict("Já existe um cliente com este CPF.");
                }

                _context.InserirCliente(cliente);

                return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest("ID do cliente não coincide.");
            }

            // Verifica se o cliente existe
            var existingCliente = await _context.Clientes.FindAsync(id);
            if (existingCliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            // Verifica se o CPF já está em uso por outro cliente
            var existingCpfCliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Cpf == cliente.Cpf && c.Id != id);

            if (existingCpfCliente != null)
            {
                return Conflict("Já existe um cliente com este CPF.");
            }

         
            try
            {
                _context.AtualizarCliente(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.Id == id))
                {
                    return NotFound("Cliente não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteCliente")]
        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado");
            }

            _context.DeletarCliente(id);

            return NoContent();
        }

        



    }
}
